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
        foreach (BackGround bg in backGrounds)
        {
            bg.backGroundGO.SetActive(false);
        }

        // スクリプトの引数と同じ名前の背景を表示
        BackGround matchedBackground = backGrounds.FirstOrDefault(x => x.name == row.args[0]);
        matchedBackground.backGroundGO.SetActive(true);
    
        yield break;
    }
}
