using UnityEngine;
using System;

[System.Serializable]
public class ClickGameObject
{
    [Header("クリック対象の名前")]
    public string name;

    [Header("クリック対象のゲームオブジェクト")]
    public GameObject gameObject;

    public event Action<GameObject> OnClicked;

    public void NotifyClicked()
    {
        OnClicked?.Invoke(gameObject);
    }
}
