using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StackMachine : MonoBehaviour
{
    [Header("ここにEventDataをアタッチ")]
    [SerializeField]
    EventData eventData;

    List<ICommandExecutor> executors = new List<ICommandExecutor>();

    void Awake()
    {
        executors.AddRange(GetComponents<ICommandExecutor>());
    }

    void Start()
    {
        RunScript("test3");
    }

    void Update()
    {
        
    }

    // スクリプトを呼び出して実行する
    void RunScript(string scriptName)
    {
        Debug.Log("RunScript");
        ScriptRow script = eventData.script.Find(s => s.name == scriptName);
        StartCoroutine(Run(script));
    }

    // スクリプトを指定して実行を開始する
    IEnumerator Run(ScriptRow script)
    {
        Debug.Log("Run");
        foreach (var row in script.csvList)
        {
            yield return ExecuteRow(row);
        }
    }

    // スクリプトを１行ずつ実行していく
    IEnumerator ExecuteRow(CsvRow row)
    {
        Debug.Log("" + row.command + " が実行されました");
        foreach(var exec in executors)
        {
            if (exec.CanExecute(row.command))
            {
                yield return StartCoroutine(exec.Execute(row));
                yield break;
            }
        }
    }


}
