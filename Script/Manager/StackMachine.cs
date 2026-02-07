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
        RunScript("test2");
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

    IEnumerator Run(ScriptRow script)
    {
        Debug.Log("Run");
        foreach (var row in script.csvList)
        {
            yield return ExecuteRow(row);
        }
    }

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
