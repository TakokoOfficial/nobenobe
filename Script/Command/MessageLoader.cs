using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageLoader : MonoBehaviour, ICommandExecutor
{
    // コマンドの文言を定義
    // SayはMessageLoaderが担当する
    public bool CanExecute(string command)
    {
        return command == "Say";
    }

    public IEnumerator Execute(CsvRow row)
    {
        Debug.Log("" + row.args[0]);
        yield return new WaitForSeconds(1);
    }
}
