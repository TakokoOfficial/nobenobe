// セリフを修飾するモディファイアの開始位置と終了位置、モディファイアの種類を格納する

using UnityEngine;

public class ModifierRange
{
    public int start;
    public int end;
    public ITextModifier modifier;

    public ModifierRange(int start, int end, ITextModifier itm)
    {
        this.start = start;
        this.end = end;
        this.modifier = itm;
    }

    public bool Contains(int index)
    {
        return index >= start && index < end;
    }
}