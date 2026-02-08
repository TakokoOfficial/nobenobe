using TMPro;
using UnityEngine;

public class BoldModifier : MonoBehaviour, ITextModifier
{
    public bool CanApply(string tag) => tag == "bold";

    [Range(1.0f, 2.0f)]
    public float scale = 1.2f;

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
        var verts = meshInfo.vertices;

        // 文字の中心点
        Vector3 center =
            (verts[vertIndex + 0] +
             verts[vertIndex + 2]) * 0.5f;

        for (int i = 0; i < 4; i++)
        {
            verts[vertIndex + i] =
                center + (verts[vertIndex + i] - center) * scale;
        }

        meshInfo.mesh.vertices = verts;
    }
}
