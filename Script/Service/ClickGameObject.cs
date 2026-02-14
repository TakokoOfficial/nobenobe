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
    public event Action<GameObject> OnHovered;

    public void NotifyClicked()
    {
        OnClicked?.Invoke(gameObject);
    }

    public void NotifyHovered()
    {
        OnHovered?.Invoke(gameObject);
    }
}