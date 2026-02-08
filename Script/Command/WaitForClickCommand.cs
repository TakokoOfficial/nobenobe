using UnityEngine;
using System.Collections;

public class WaitForClickCommand : MonoBehaviour, ICommandExecutor
{
    [SerializeField] InputGetter inputGetter;

    public bool CanExecute(string command)
    {
        return command == "Stay";
    }

    public IEnumerator Execute(CsvRow row)
    {
        // 2つ目の引数を取得
        string targetName = row.args[0];

        Debug.Log($"Stay: {targetName} のクリック待ち");

        yield return inputGetter.WaitForClick(targetName);
    }
}