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
    if (EventSystem.current == null) return;

    PointerEventData pointerData = new PointerEventData(EventSystem.current)
    {
        position = Input.mousePosition
    };

    var results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(pointerData, results);

    if (results.Count > 0)
    {
        var top = results[0].gameObject;

        foreach (var cgo in clickGameObjects)
        {
            if (cgo.gameObject == top)
            {
                cgo.NotifyHovered();
            }
        }
    }

    if (Input.GetMouseButtonDown(0))
    {
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


public IEnumerator WaitForInteraction(string name, string mode)
{
    ClickGameObject target = GetClickObjectByName(name);

    if (target == null)
    {
        Debug.LogError($"ClickGameObject '{name}' が見つかりません");
        yield break;
    }

    bool done = false;

    void OnClick(GameObject go) => done = true;
    void OnHover(GameObject go) => done = true;

    if (mode == "on")
    {
        target.OnHovered += OnHover;
    }
    else
    {
        target.OnClicked += OnClick;
    }

    yield return new WaitUntil(() => done);

    target.OnHovered -= OnHover;
    target.OnClicked -= OnClick;
}
}