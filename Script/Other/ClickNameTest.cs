using UnityEngine;
using UnityEngine.EventSystems;

public class ClickNameTest : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + gameObject.name);
    }
}