using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventData : MonoBehaviour
{
    // 命令列（イベントスクリプトとかアイテムスクリプト）を格納するところ
    [SerializeField]
    public List<ScriptRow> script = new List<ScriptRow>();


    void Start()
    {
        ReadCsv("eventScript");
    }

    void Update()
    {
        
    }

    // Resourceフォルダにある、fileNameフォルダ下のcsvを一括読み取り
    public void ReadCsv(string fileName){
        TextAsset[] csvFiles = Resources.LoadAll<TextAsset>(fileName);
        foreach(TextAsset csv in csvFiles)
        {
            Debug.Log("" + csv.name);
            script.Add(ParseCsv(csv.text, csv.name));
        }
    }


    // CSVを１行ずつ分解してScriptRowに格納していく
    public ScriptRow ParseCsv(string text, string name)
    {
        ScriptRow sr = new ScriptRow(name);
        
        var lines = text
        .Replace("\r\n", "\n")
        .Replace("\r", "\n")
        .Split('\n');

        foreach(var line in lines)
        {
            if(string.IsNullOrEmpty(line)) continue;
            sr.AddCommand(ParseLine(line));
        }

        return sr;
    }


    // スクリプトをカンマ区切りで分解してCsvRowに格納していく
    private CsvRow ParseLine(string line)
    {
        var cols = line.Split(',');
        string command = cols[0].Trim();

        List<string> argsList = new List<string>();

        for(int i = 1; i < cols.Length; i++)
        {
            argsList.Add(cols[i].Trim());
        }

        return new CsvRow(command, argsList);
    }
}
