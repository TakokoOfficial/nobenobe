// コマンドを定義するインタフェース
// 引数と自身のコマンド名が一致しているか、具体的な処理内容の２つを必ず実装する必要アリ

using System.Collections;
using System.Collections.Generic;

public interface ICommandExecutor
{
    bool CanExecute(string command);
    IEnumerator Execute(CsvRow row);
}