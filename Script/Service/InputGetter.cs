using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InputGetter : MonoBehaviour
{
    [SerializeField]
     ClickGameObject[] clickGameObjects;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current == null) return;

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                foreach (var cgo in clickGameObjects)
                {
                    if (cgo.gameObject == result.gameObject)
                    {
                        cgo.NotifyClicked();
                        return;
                    }
                }
            }
        }
        
    }

    public ClickGameObject GetClickObjectByName(string name)
    {
        foreach (var cgo in clickGameObjects)
        {
            if (cgo.name == name)
                return cgo;
        }
        return null;
    }


    public IEnumerator WaitForClick(string name)
    {
        ClickGameObject target = GetClickObjectByName(name);

        if (target == null)
        {
            Debug.LogError($"ClickGameObject '{name}' が見つかりません");
            yield break;
        }

        bool clicked = false;

        void OnClick(GameObject go)
        {
            clicked = true;
        }

        target.OnClicked += OnClick;

        yield return new WaitUntil(() => clicked);

        target.OnClicked -= OnClick;
    }
}