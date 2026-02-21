using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;


// TMProやUnityEngine.UIとの橋渡しをするのみ
// 細かいデータは全てItemManager.csが管理、操作する

public class item : MonoBehaviour
{
    [Header("ここにアイテム名のTMProをアタッチ")]
    public TextMeshProUGUI itemNameText;

    [Header("ここに説明のTMProをアタッチ")]
    public TextMeshProUGUI itemInfoText;

    [Header("ここにアイテムの画像をアタッチ")]
    public Image itemSprite;

    [Header("ここにアイテムのトリガーをアタッチ")]
    public Button itemTrigger;

    // クリックを通知するイベント
    public event Action<item> OnItemClicked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemTrigger.onClick.AddListener(HandleClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeItemName(string itemName)
    {
        itemNameText.text = itemName;
    }

    public void changeItemInfo(string itemInfo)
    {
        itemInfoText.text = itemInfo;
    }

    public void changeItemIcon(Sprite sprite)
    {
        itemSprite.sprite = sprite;
    }

    void HandleClick()
    {
        OnItemClicked?.Invoke(this);
    }
}
