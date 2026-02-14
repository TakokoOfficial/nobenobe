using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaitForClickCommand : MonoBehaviour, ICommandExecutor
{
    [SerializeField] InputGetter inputGetter;

    public bool CanExecute(string command)
    {
        return command == "Stay";
    }

    public IEnumerator Execute(CsvRow row)
    {
        string targetName = row.args[0];
        string mode = row.args.Count > 1 ? row.args[1] : "";

        Debug.Log($"Stay: {targetName} / mode={mode}");

        yield return inputGetter.WaitForInteraction(targetName, mode);
    }
}