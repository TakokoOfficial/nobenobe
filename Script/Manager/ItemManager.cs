using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

[System.Serializable]
public class ItemImage
{
    [Header("画像の名前を入力")]
    public string name;
    [Header("対応する画像をアタッチ")]
    public Sprite sprite;
}

[System.Serializable]
public class ItemData
{
    [Header("アイテムの名称")]
    public string itemName;

    [Header("アイテムは絶対に壊れないか？")]
    public bool isImmortal;

    [Header("アイテムの最大耐久値")]
    public int maxDurabilityValue;

    [Header("アイテムの現在耐久値")]
    public int durabilityValue;

    [Header("アイテムの壊れる確率（1~100）")]
    public int fragility;

    [Header("アイテムのゲームオブジェクト")]
    public GameObject itemGO;
    
    [Header("item.cs")]
    public item itemCS;
}

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    [Header("画像データの設定")]
    public List<ItemImage> itemImages = new List<ItemImage>();

    [Header("ここに所持アイテムが入る")]
    public List<ItemData> items = new List<ItemData>();

    [Header("ここにItemScriptが入る")]
    public ScriptRow sr;

    [Header("ここにEventDataをアタッチ")]
    public EventData eventData;

    [Header("ここにアイテムのプレハブをアタッチ")]
    public GameObject itemGO;

    [Header("ここにアイテム欄をアタッチ")]
    public Transform itemStorage;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // アイテムのデータをEventData.csから取得する
        sr = eventData.GetScriptRow("itemData");   

        // テスト
        AddItem("dripRod");
        AddItem("dripRod");
        AddItem("dripRod");
        AddItem("dripRod");
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void AddItem(string ItemName) 
    {
        // リストitemsにItemDataをAddした上でitemStorageにアイテムを追加する

        // srからGetCsvRows(0,ItemName)でCsvRow crを取ってくる
        // 取ってきたcrは以下の例のようになる
        // dripRod,点滴ロッド,次の走る距離1.5倍,点滴液を消費する,1,5,4,20
        // これらを用いてリストitemsにAddするItemDataを構築する

        // 上の例を用いて構築されるItemDataを例示する

        // ItemDataのitemNameは dripRod
        // ItemDataのisImmortalは 1
        // ItemDataのmaxDurabilityValueは 5
        // ItemDataのdurabilityValueは 4
        // ItemDataのfragilityは 20

        // ItemDataのitemGOはitemGOをInstantiate
        // Instantiate先の親オブジェクトはitemStorage
        // ItemDataのitemCSはitemGOからGetComponent

        // itemCSはUnityEngineのUIを操作するクラスなのでこれを使ってInstantiateしたゲームオブジェクトを更新する
        // itemCS.changeItemName("点滴ロッド") でアイテム名を反映
        // itemCS.changeItemInfo("次の走る距離1.5倍\n点滴液を消費する")でアイテム説明を反映
        // itemCS.changeItemIcon(Sprite)でアイテム画像を変更・引数として渡す画像データはItemImagesに画像データと画像名（dripRod）が対応する形で格納されているので検索して画像データを持ってくる
        
        if (sr == null)
        {
            Debug.LogError("ScriptRow is null");
            return;
        }

        // commandがItemNameと一致するCsvRowを探す
        CsvRow targetRow = null;

        foreach (var row in sr.csvList)
        {
            if (row.command == ItemName)
            {
                targetRow = row;
                break;
            }
        }

        if (targetRow == null)
        {
            Debug.LogWarning($"Item not found in CSV: {ItemName}");
            return;
        }

        // CsvRowのargsを取得
        List<string> args = targetRow.args;

        // データ構築
        ItemData newItem = new ItemData();

        newItem.itemName = ItemName;
        newItem.isImmortal = args[3] == "1";
        newItem.maxDurabilityValue = int.Parse(args[4]);
        newItem.durabilityValue = int.Parse(args[5]);
        newItem.fragility = int.Parse(args[6]);

        // プレハブ生成
        GameObject instance = Instantiate(itemGO, itemStorage);
        newItem.itemGO = instance;

        // item.cs取得
        item itemCS = instance.GetComponent<item>();
        newItem.itemCS = itemCS;

        // UI反映
        itemCS.changeItemName(args[0]);

        string description = args[1] + "\n" + args[2];
        itemCS.changeItemInfo(description);

        // 画像検索
        Sprite iconSprite = null;

        foreach (var img in itemImages)
        {
            if (img.name == ItemName)
            {
                iconSprite = img.sprite;
                break;
            }
        }

        if (iconSprite != null)
        {
            itemCS.changeItemIcon(iconSprite);
        }

        // itemsリストに追加
        items.Add(newItem);

        // クリックイベント登録
        itemCS.OnItemClicked += HandleItemClicked;
    }

    // アイテム使用時の処理
    private void HandleItemClicked(item clickedItem)
    {
        ItemData itemData = itemToItemData(clickedItem);
        
        string clickedItemName = itemData.itemName;
        Debug.Log("" + clickedItemName);

        // 具体的な処理を書く
        switch (clickedItem)
        {
            
        }

        // 破壊されるか？
        itemDestroy(itemData);
        
    }


    private ItemData itemToItemData(item targetItem)
    {
        // itemsの中から引数のitemを持つitemDataを削除
        foreach (var data in items)
        {
            if (data.itemCS == targetItem)
            {
                return data;
            }
        }

        Debug.LogWarning("ItemData not found for clicked item.");
        return null;        
    }

    // アイテム破壊時の処理
    private void itemDestroy(ItemData itemData)
    {
        if (itemData == null) return;

        // UIオブジェクト破壊
        if (itemData.itemGO != null)
        {
            Destroy(itemData.itemGO);
        }

        // Listから削除
        items.Remove(itemData);
    }
}
