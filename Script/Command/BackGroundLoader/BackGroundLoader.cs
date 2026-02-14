using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BackGroundLoader : MonoBehaviour, ICommandExecutor
{
    [SerializeField]
    [Header("背景設定")]
    public List<BackGround> backGrounds;

    public bool CanExecute(string command)
    {
        return command == "ChangeBG";
    }
    public IEnumerator Execute(CsvRow row)
    {
        // その他の背景を非表示に
        allBackGroundFalse();

        // スクリプトの引数と同じ名前の背景を表示
        BackGround matchedBackground = backGrounds.FirstOrDefault(x => x.name == row.args[0]);
        matchedBackground.backGroundGO.SetActive(true);
    
        yield break;
    }

    public void allBackGroundFalse()
    {
        // その他の背景を非表示に
        foreach (BackGround bg in backGrounds)
        {
            bg.backGroundGO.SetActive(false);
        }
    }

    public List<BackGround> GetBackGrounds()
    {
        return backGrounds;
    }

    public BackGround GetBackGround(string name)
    {
        foreach (BackGround bg in backGrounds)
        {
            if (bg.name == name)
            {
                return bg;
            }
        }

        Debug.LogWarning($"背景が見つかりません: {name}");
        return null;
    }
}
