using UnityEngine;

[System.Serializable]
public class Animation
{
    [Header("アニメーションの名称を入力")]
    public string name;

    [Header("アニメーターをアタッチ")]
    public Animator animator;

    [Header("再生したいステート名を入力")]
    public string stateName;
    
}
