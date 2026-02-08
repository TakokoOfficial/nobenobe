// メッセージの表示される場所を保管するクラス
// TextMeshProとテキストを表示する場所の名前を対応させる

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class MessagePlace
{
    [SerializeField]
    [Header("テキストの場所の名前を入力")]
    public string textName;    

    [SerializeField]
    [Header("ここにTMPをアタッチ")]
    public TextMeshProUGUI tmp;

    [SerializeField]
    [Header("ここにModifierRangeが格納される")]
    public List<ModifierRange> mrList;

    [SerializeField]
    [Header("可視化される文字のindexを格納")]
    public List<int> visibleToRawIndex;

}
