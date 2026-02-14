using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameLoader : MonoBehaviour, ICommandExecutor
{
    [Header("ここにGameManagerをアタッチ")]
    public GameManager gameManager;

    public bool CanExecute(string command)
    {
        return command == "Game";
    }
    public IEnumerator Execute(CsvRow row)
    {
        switch (row.args[0])
        {
            case "run":
                {
                    gameManager.LetsRun();
                    yield break;
                }
            case "sleep":
                {
                    gameManager.LetsSleep();
                    yield break;
                }
            case "return":
                {
                    gameManager.LetsReturn();
                    yield break;
                }
            case "speedUp":
                {
                    gameManager.SpeedUp();
                    yield break;
                }
            case "sleeping":
                {
                    gameManager.Sleeping();
                    yield break;
                }
            case "health":
                {
                    gameManager.ChangeHealth(int.Parse(row.args[1]));
                    yield break;
                }
            case "stageStart":
                {
                    gameManager.StageStart();
                    yield break;
                }


        }

        yield break;
    }
}
