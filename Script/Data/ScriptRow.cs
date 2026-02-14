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

    // ScriptRowに格納されたCsvRow群の中から、args[i]とnameに一致するCsvRowのリストを返す
    public List<CsvRow> GetCsvRows(int i, string name)
    {
        List<CsvRow> result = new List<CsvRow>();

        foreach (var row in csvList)
        {
            // args が存在し、i番目があるかチェック
            if (row.args != null && row.args.Count > i)
            {
                if (row.args[i] == name)
                {
                    result.Add(row);
                }
            }
        }
        return result;
    }

    // ScriptRowに格納されたCsvRow群の中から、args[i]とnameに一致するCsvRowのリストを返す
    public List<CsvRow> GetCsvRows(int i, int num)
    {
        List<CsvRow> result = new List<CsvRow>();

        foreach (var row in csvList)
        {
            if (row.args != null && row.args.Count > i)
            {
                int value;

                // 数値に変換できるかチェック
                if (int.TryParse(row.args[i], out value))
                    {
                    if (value == num)
                    {
                        result.Add(row);
                    }
                }
            }
        }
        return result;
    }
}
