using System.Collections;
using System.Collections.Generic;

public interface ICommandExecutor
{
    bool CanExecute(string command);
    IEnumerator Execute(CsvRow row);
}