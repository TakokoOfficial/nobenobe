using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TextEffectController : MonoBehaviour
{
    List<MessagePlace> places;

    // MessageLoader から呼ばれる
    public void SetMessagePlace(List<MessagePlace> mp)
    {
        places = mp;
    }

    void LateUpdate()
    {
        // メッセージの配置場所が指定されていなければ処理中断
        if (places == null) return;


        // メッセージを表示する場所それぞれについて
        foreach (var place in places)
        {
            // メッセージ場所のTextMeshProを取得
            var tmp = place.tmp;

            // TextMeshProがアタッチされていなければパス
            if (place.tmp == null) continue;
            
            // もしTextMeshProが何も描画していなければパス
            if (string.IsNullOrEmpty(tmp.text)) continue;

            // TextMeshProを更新
            tmp.ForceMeshUpdate();

            // TextMeshProのtextInfoを取得
            // textInfoには元の文字列の位置だったりインデックスだったりが格納されている
            var textInfo = tmp.textInfo;

            // 表示されている文字数を取得
            int visibleCount = tmp.maxVisibleCharacters;

            // それぞれの文字について
            for (int v = 0; v < visibleCount; v++)
            {
                // 表示予定の最大文字数を超えていたらパス
                if (v >= place.visibleToRawIndex.Count) continue;

                // 実際に表示される文字が何文字目に表示されるかを取得
                int rawIndex = place.visibleToRawIndex[v];

                // ModifierRangeを適用
                foreach (var range in place.mrList)
                {
                    // ModifierRangeが対象の文字を装飾することになっていたら
                    if (range.Contains(rawIndex))
                    {
                        // 適用する
                        range.modifier.Apply(
                            tmp,
                            v,
                            rawIndex,
                            textInfo
                        );
                    }
                }
            }

            // TextMeshProを適用 
            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
    }
}
