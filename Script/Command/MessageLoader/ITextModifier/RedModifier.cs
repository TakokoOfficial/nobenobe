using TMPro;
using UnityEngine;

public class RedModifier : MonoBehaviour, ITextModifier
{
    public bool CanApply(string tag) => tag == "red";

    public Color32 color = new Color32(255, 0, 0, 255);

    public void Apply(
        TMP_Text text,
        int visibleCharIndex,
        int rawIndex,
        TMP_TextInfo textInfo
    )
    {
        if (visibleCharIndex >= textInfo.characterCount) return;

        var charInfo = textInfo.characterInfo[visibleCharIndex];
        if (!charInfo.isVisible) return;

        int matIndex = charInfo.materialReferenceIndex;
        int vertIndex = charInfo.vertexIndex;

        var meshInfo = textInfo.meshInfo[matIndex];
        var colors = meshInfo.colors32;

        colors[vertIndex + 0] = color;
        colors[vertIndex + 1] = color;
        colors[vertIndex + 2] = color;
        colors[vertIndex + 3] = color;

        meshInfo.mesh.colors32 = colors;
    }
}
