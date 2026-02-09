using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StopLoader : MonoBehaviour, ICommandExecutor
{
    public bool CanExecute(string command)
    {
        return command == "Stop";
    }
    public IEnumerator Execute(CsvRow row)
    {
        Debug.Log(row.args[0]);
        // スクリプトの引数と同じms秒待機
        if (float.TryParse(row.args[0], out float milliseconds))
        {
            float seconds = milliseconds / 1000f;
            yield return new WaitForSeconds(seconds);
        }
        else
        {
            // エラー
            Debug.LogWarning("Stopコマンドでエラーが発生しています！");
            yield break;
        }
    }
}
