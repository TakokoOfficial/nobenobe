using TMPro;
using UnityEngine;

public class WaveModifier : MonoBehaviour, ITextModifier
{
    public bool CanApply(string tag) => tag == "wave";

    public float amplitude = 5f;
    public float frequency = 3f;

    public float interval = 0.5f;

public void Apply(
    TMP_Text text,
    int visibleCharIndex,
    int rawIndex,
    TMP_TextInfo textInfo
)
{
    var charInfo = textInfo.characterInfo[visibleCharIndex];
    if (!charInfo.isVisible) return;

    int matIndex = charInfo.materialReferenceIndex;
    int vertIndex = charInfo.vertexIndex;

    var meshInfo = textInfo.meshInfo[matIndex];
    var verts = meshInfo.vertices;

    float wave = Mathf.Sin(Time.time * frequency + visibleCharIndex * interval) * amplitude;
    Vector3 offset = new Vector3(0, wave, 0);

    verts[vertIndex + 0] += offset;
    verts[vertIndex + 1] += offset;
    verts[vertIndex + 2] += offset;
    verts[vertIndex + 3] += offset;

    // ★必須
    meshInfo.mesh.vertices = verts;
}

}
