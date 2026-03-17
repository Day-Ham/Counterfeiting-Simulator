using System.Collections.Generic;
using UnityEngine;

public class UITransitionManager : MonoBehaviour
{
    [SerializeField] private UITransitionManagerValue managerValue;

    private readonly List<UITransitionElement> uiTransitionElements = new();

    private void OnEnable()
    {
        managerValue.Value = this;
        managerValue.ResolvePending();
    }

    public void Register(UITransitionElement element)
    {
        if (element == null) return;

        if (!uiTransitionElements.Contains(element))
        {
            uiTransitionElements.Add(element);
        }
    }

    public void Unregister(UITransitionElement element)
    {
        if (uiTransitionElements.Contains(element))
        {
            uiTransitionElements.Remove(element);
        }
    }

    public void MoveAllOut()
    {
        foreach (var uiTransitionElement in uiTransitionElements)
        {
            uiTransitionElement.MoveOut();
        }
    }

    public void MoveAllIn()
    {
        foreach (var uiTransitionElement in uiTransitionElements)
        {
            uiTransitionElement.MoveIn();
        }
    }
}
