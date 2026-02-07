// 命令列の格納方法を定義するクラス
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CsvRow{
    public string command;
    public List<string> args;

    public CsvRow(string command, List<string> args)
    {
        this.command = command;
        this.args = args;
    }
}