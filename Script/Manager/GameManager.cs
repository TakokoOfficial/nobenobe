using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("ここにStackMachineをアタッチ")]
    [SerializeField]
    public StackMachine stackMachine;

    [Header("ここにEventDataをアタッチ")]
    public EventData eventData;

    [Header("ここにCountUpをアタッチ")]
    public CountUp countUp;

    [Header("ここにBackGroundLoaderをアタッチ")]
    public BackGroundLoader backGroundLoader;

    [Header("ここに場所のテキストをアタッチ")]
    public TextMeshProUGUI placeText;

    [Header("致死率を表示するテキストをアタッチ")]
    public TextMeshProUGUI deathPercentageText;

    [Header("致死率を表示するテキストをアタッチ")]
    public int deathPercentage;

    [Header("イベント発生率を表示するテキストをアタッチ")]
    public TextMeshProUGUI eventPercentageText;

    [Header("イベント発生率を表示するテキストをアタッチ")]
    public int eventPercentage;

    [Header("走る予定の場所")]
    public List<CsvRow> stageCsvRows = new List<CsvRow>();

    [Header("現在いる場所")]
    public string nowPlace;


    [Header("現在のステージ")]
    public int stageNow = 0;

    [Header("現在の場所から進んだ回数")]
    public int speedUpTime = 0;

    [Header("天使は存命か")]
    public bool isLiveAngel = true;

    [Header("現在の最大体力")]
    public int maxhealth = 6;

    [Header("現在の体力")]
    public int health = 6;

    [Header("体力バーのアニメーションをアタッチ")]
    public Animator healthAnim;


    [SerializeField]
    ScriptRow stageDataSR = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stageDataSR = eventData.GetScriptRow("stageData");
        StageStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StageStart()
    {
        int stage = stageNow;
        stackMachine.StopAllRunningCoroutines();

        stackMachine.RunScript("goButtonClick");
        stackMachine.RunScript("sleepButtonClick");
        stackMachine.RunScript("returnButtonClick");

        if(stageCsvRows.Count == 0)
        {
            StageFill(stage);            
        }

        CsvRow nextStage = StageGet();
        StageAdd(stage);

        // 場所名の反映
        placeText.text = nextStage.command;

        // 致死率の設定
        deathPercentage = int.Parse(nextStage.args[3]);
        countUp.CountUpStart(deathPercentageText,deathPercentage);

        // イベント率の設定
        eventPercentage = int.Parse(nextStage.args[5]);
        countUp.CountUpStart(eventPercentageText,eventPercentage);

        // 背景の設定
        backGroundLoader.allBackGroundFalse();
        backGroundLoader.GetBackGround(nextStage.args[2]).backGroundGO.SetActive(true);

        // アニメーションを再生
        stackMachine.RunScript("stageStart");
    }


    // リストの中から次のステージを持ってくる
    public CsvRow StageGet()
    {
        if (stageCsvRows.Count == 0)
        {
            Debug.LogWarning("ストックが空");
            return null;
        }

        CsvRow row = stageCsvRows[0];
        stageCsvRows.RemoveAt(0);

        return row;
    }

    // ステージ番号を指定して、重み付きランダムなステージを設定する
    // 同じステージは連続して選ばれない
    public void StageAdd(int stage)
    {
        // リストにステージを追加する
        // args[0] == stage のものを取得
        List<CsvRow> csvRows = stageDataSR.GetCsvRows(0, stage);

        if (csvRows == null || csvRows.Count == 0)
        {
            Debug.LogWarning("該当ステージなし");
            return;
        }

        // 直前ステージ取得（連続防止用）
        string lastStageName = null;
        if (stageCsvRows.Count > 0)
        {
            lastStageName = stageCsvRows[stageCsvRows.Count - 1].command;
        }

        // 重み合計（連続するものは除外）
        int totalWeight = 0;
        foreach (var row in csvRows)
        {
            if (row.command == lastStageName) continue;

            if (int.TryParse(row.args[1], out int weight))
                totalWeight += weight;
        }

        if (totalWeight == 0)
        {
            Debug.LogWarning("重みがすべて0、または候補が1つのみ");
            return;
        }

        int randomValue = Random.Range(0, totalWeight);

        int currentSum = 0;
        CsvRow selected = null;

        foreach (var row in csvRows)
        {
            if (row.command == lastStageName) continue;

            if (int.TryParse(row.args[1], out int weight))
            {
                currentSum += weight;

                if (randomValue < currentSum)
                {
                    selected = row;
                    break;
                }
            }
        }

        if (selected != null)
        {
            stageCsvRows.Add(selected);
        }
    }

    // リストを一度空にした後、ステージで埋める
    public void StageFill(int stage)
    {
        StageReset();

        int preloadCount = 5;

        for (int i = 0; i < preloadCount; i++)
        {
            StageAdd(stage);
        }
    }

    // リストのステージをすべて消し飛ばす
    public void StageReset()
    {
        stageCsvRows.Clear();
    }





    public void LetsRun()
    {
        stackMachine.RunScript("run");
        ChangeHealth(-1);
    }

    public void LetsSleep()
    {
        stackMachine.RunScript("sleep");
    }

    public void LetsReturn()
    {
        stackMachine.RunScript("return");
        ChangeHealth(-1);
        StageStart();
    }

    // 移動中、主人公をクリックした際に呼び出される
    // １ステージに３回呼び出される
    public void SpeedUp()
    {
        speedUpTime++;
        if(speedUpTime >= 3)
        {
            // アクシデントが起こったり起こらなかったりする
            // アクシデントの発生確率を計算する
            int randomI = Random.Range(0, 100);

            // 失敗
            if (deathPercentage > randomI)
            {
                AccidentOccurred();
            }
            else
            {
                stackMachine.RunScript("runSuccess");                
            }
            speedUpTime = 0;
        }
    }

    // 就寝中、主人公をクリックした際に呼び出される
    public void Sleeping()
    {
        Debug.Log(" ほげ");
        // アクシデントが起こったり起こらなかったりする
        // アクシデントの発生確率を計算する
        int randomI = Random.Range(0, 100);

        // 失敗
        if (deathPercentage > randomI)
        {
            AccidentOccurred();
        }
        else
        {
            stackMachine.RunScript("sleepSuccess");
        }
    }


    public void AccidentOccurred()
    {
        // stackMachineのスレッド全停止する
        Debug.Log("死");

        // 天使が生きているか否かで分岐
        if (isLiveAngel)
        {
            AngelDie();
        }
        else
        {
            HeroDie();
        }
    }

    public void AngelDie()
    {
        // 天使を場面に合った方法で殺す

    }

    public void HeroDie()
    {
        // 主人公を場面に合った方法で殺す
        // ゲームオーバー画面を出す

    }

    public void AllReset()
    {
        // すべてをリセットしてゲームやり直し

    }


    // 主人公の体力を変更する際に呼び出す
    public void ChangeHealth(int amount)    
    {
        // 変更前のhealthを保存
        int prevHealth = health;

        // 体力変更
        health += amount;
        health = Mathf.Clamp(health, 0, maxhealth);

        // 変更前 index
        float prevRatio = (float)prevHealth / maxhealth;
        int prevIndex = Mathf.Clamp(Mathf.FloorToInt(prevRatio * 6f), 0, 6);

        // 変更後 index
        float ratio = (float)health / maxhealth;
        int index = Mathf.Clamp(Mathf.FloorToInt(ratio * 6f), 0, 6);

        if (amount >= 0)
        {
            // 回復時：変更前〜現在まで再生
            for (int i = prevIndex; i <= index; i++)
            {   
                string animName = $"health{i}";
                Debug.Log(animName);
                healthAnim.Play(animName);
            }
        }
        else
        {
            // 減少時：現在の段階のみ再生
            string animName = $"health{index} 1";
            Debug.Log(animName);
            healthAnim.Play(animName);
        }
    }
}

