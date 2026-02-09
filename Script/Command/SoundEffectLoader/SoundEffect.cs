using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    [Header("音の名称を入力")]
    public string name;

    [Header("音をアタッチ")]
    public AudioClip audioClip;

    [Header("音量を設定")]
    public float volume;

    [Header("音源の発生間隔を設定")]
    public float interval;
}
