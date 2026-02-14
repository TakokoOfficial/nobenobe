using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StackMachine : MonoBehaviour
{
    [Header("ここにEventDataをアタッチ")]
    [SerializeField]
    EventData eventData;

    List<ICommandExecutor> executors = new List<ICommandExecutor>();

    void Awake()
    {
        executors = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<ICommandExecutor>()
            .ToList();
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    // スクリプトを呼び出して実行する
    public void RunScript(string scriptName)
    {
        Debug.Log("RunScript = " + scriptName);
        ScriptRow script = eventData.script.Find(s => s.name == scriptName);
        StartCoroutine(Run(script));
    }

    // スクリプトを指定して実行を開始する
    IEnumerator Run(ScriptRow script)
    {
        Debug.Log("Run = " + script.name);
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

    public void StopAllRunningCoroutines()
    {
        Debug.Log("StackMachine: すべてのコルーチンを停止");
        StopAllCoroutines();
        foreach (var exec in executors)
        {
            if (exec is MonoBehaviour mb)
            {
                mb.StopAllCoroutines();
            }
        }
    }
}
