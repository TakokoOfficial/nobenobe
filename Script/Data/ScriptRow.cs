// 命令列群の格納方法を定義するクラス
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ScriptRow{
    public string name = "";
    public List<CsvRow> csvList = new List<CsvRow>();

    public ScriptRow(string name)
    { 
        this.name = name;
    }

    public void AddCommand(string command, List<string> args)
    {
        csvList.Add(new CsvRow(command, args));
    }

    public void AddCommand(CsvRow cr)
    {
        csvList.Add(cr);
    }
}
