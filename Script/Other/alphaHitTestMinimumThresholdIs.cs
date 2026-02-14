
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class alphaHitTestMinimumThresholdIs :
    MonoBehaviour
{
    private void Awake()
    {
        var image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = 0.01f;
    }
}
