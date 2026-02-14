using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventData : MonoBehaviour
{
    // 命令列（イベントスクリプトとかアイテムスクリプト）を格納するところ
    [SerializeField]
    public List<ScriptRow> script = new List<ScriptRow>();

    void Awake()
    {
        ReadCsv("eventScript");
        ReadCsv("stageScript");
        ReadCsv("uiScript");

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Resourceフォルダにある、fileNameフォルダ下のcsvを一括読み取り
    public void ReadCsv(string fileName){
        TextAsset[] csvFiles = Resources.LoadAll<TextAsset>(fileName);
        foreach(TextAsset csv in csvFiles)
        {
            script.Add(ParseCsv(csv.text, csv.name));
        }
    }


    // CSVを１行ずつ分解してScriptRowに格納していく
    private ScriptRow ParseCsv(string text, string name)
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


    // 引数とScriptRowのnameと一致するScriptRowを返す
    public ScriptRow GetScriptRow(string name)
    {
    foreach (ScriptRow sr in script)
        {
            if (sr.name == name)
            {
                return sr;
            }
        }

        Debug.LogWarning($"ScriptRow not found: {name}");
        return null;
    }
}
