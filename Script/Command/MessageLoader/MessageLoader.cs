using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class MessageLoader : MonoBehaviour, ICommandExecutor
{
    [SerializeField]
    [Header("ここにTMP関係の情報を入力")]
    public List<MessagePlace> mp;


    [SerializeField]
    [Header("ここにTextEffectControllerをアタッチ")]
    public TextEffectController tec;

    [SerializeField]
    [Header("ここにTextModifierをアタッチ")]
    List<MonoBehaviour> modifierBehaviours;

    // interfaceはアタッチできないのでMonoBehaviourを介す
    List<ITextModifier> modifiers = new();
    

    void Awake()
    {
        modifiers.Clear();

        modifiers = modifierBehaviours
            .OfType<ITextModifier>()
            .ToList();

        tec.SetMessagePlace(mp);
            
    }


    // コマンドの文言を定義
    // SayはMessageLoaderが担当する
    public bool CanExecute(string command)
    {
        return command == "Say";
    }

    public IEnumerator Execute(CsvRow row)
    {
        // セリフ配置場所の選定
        MessagePlace messagePlace = SetMessagePlace(row);
        if (messagePlace == null) yield break;

        // Say,の場合はメッセージを消す
        if(row.args.Count == 0 || string.IsNullOrEmpty(row.args[0]))
        {
            ClearMessage(messagePlace);
            yield break;
        }


        messagePlace.mrList.Clear();
        
        // Modifierを適用
        SetModifier(row ,messagePlace);

        yield return StartCoroutine(
            TypeText(messagePlace)
        );
    }


    // メッセージを全リセット
    void ClearMessage(MessagePlace mp)
    {
        mp.tmp.text = "";
        mp.tmp.maxVisibleCharacters = 0;

        mp.mrList.Clear();
        mp.visibleToRawIndex.Clear();

        mp.tmp.ForceMeshUpdate();
    }


    // セリフを1文字ずつ表示する
    IEnumerator TypeText(MessagePlace mp)
    {
        var tmp = mp.tmp;
        tmp.ForceMeshUpdate();

        int total = tmp.textInfo.characterCount;
        tmp.maxVisibleCharacters = 0;

        for (int visibleIndex = 0; visibleIndex < total; visibleIndex++)
        {
            tmp.maxVisibleCharacters = visibleIndex + 1;
            
            yield return new WaitForSeconds(0.05f);
        }
    }


    // スクリプトからセリフを配置する場所を決定する
    MessagePlace SetMessagePlace(CsvRow row)
    {
        // mpチェック
        if (mp == null || mp.Count == 0)
        {
            Debug.LogError("mp が未設定");
            return null;
        }

        // 表示位置決定
        MessagePlace tmpmp;

        // 特に表示位置の指定が無ければデフォルトに設定
        if (row.args.Count < 2 || string.IsNullOrEmpty(row.args[1]))
        {
            tmpmp = mp[0];
        }
        // 指定があればMessagePlaceの名称に合うものを選ぶ
        else
        {
            tmpmp = mp.Find(m => m.textName == row.args[1]);
        }

        // MessagePlaceが設定されていなかった場合はエラーを出す
        if (tmpmp == null)
        {
            string name = row.args.Count >= 2 ? row.args[1] : "(default)";
            Debug.LogError($"MessagePlace が見つかりません: {name}");
            return null;
        }

        // もしModifierRangeがnullだったら作る
        if (tmpmp.mrList == null)
        {
            tmpmp.mrList = new List<ModifierRange>();
        }

        return tmpmp;
    }


    // 太字、色文字、波に揺れる文字などのModifierを解析、適用する
    void SetModifier(CsvRow row, MessagePlace mp)
    {
        // 生の文章raw
        string raw = row.args[0];

        // 表示予定の場所の状態をリセット
        mp.mrList.Clear();
        mp.visibleToRawIndex = new List<int>();
        string displayText = "";

        // タグを格納するスタック
        Stack<(string tag, int rawStart)> stack = new();

        // モディファイアを解析して、タグを除いた文字だけを表示するようにスタックに格納していく
        for (int rawIndex = 0; rawIndex < raw.Length; rawIndex++)
        {

            // <wave> のようなタグの始まりを見つける
            if (raw[rawIndex] == '<')
            {
                // タグの終わりを記録する
                int tagEnd = raw.IndexOf('>', rawIndex);
                if (tagEnd == -1) break;

                string tag = raw.Substring(rawIndex + 1, tagEnd - rawIndex - 1);

                // <end>であればModifierを適用
                if (tag == "end")
                {
                    var (startTag, startRaw) = stack.Pop();
                    var modifier = FindModifier(startTag);
                    if (modifier != null)
                    {
                        // ModifierRangeを対応するMessagePlaceに追加
                        mp.mrList.Add(
                            new ModifierRange(startRaw, rawIndex, modifier)
                        );
                    }
                }
                else
                {
                    stack.Push((tag, rawIndex));
                }

                rawIndex = tagEnd;
            }
            else
            {
                displayText += raw[rawIndex];
                mp.visibleToRawIndex.Add(rawIndex);
            }
        }
    mp.tmp.text = displayText;
    }
    
    // modifiersのうちtagと同じ名称のmodifierを渡す
    ITextModifier FindModifier(string tag)
    {
        // FirstOrDefault：条件に合う初めの要素に制限する
        return modifiers.FirstOrDefault(m => m.CanApply(tag));
    }
}

