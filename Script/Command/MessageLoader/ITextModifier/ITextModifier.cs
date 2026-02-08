using TMPro;

public interface ITextModifier
{
    // <bold> などのタグ名に反応するか
    bool CanApply(string tag);

    // 1文字分の見た目を変更する
    void Apply(
        TMP_Text text,
        int visibleCharIndex,
        int rawIndex,
        TMP_TextInfo textInfo
    );
}
