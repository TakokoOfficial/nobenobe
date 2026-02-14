using UnityEngine;

[System.Serializable]
public class Music
{
    [Header("音楽の名称を入力")]
    public string name;

    [Header("音楽をアタッチ")]
    public AudioClip audioClip;

    [Header("音楽の音量を入力")]
    public float volume;
    
}
