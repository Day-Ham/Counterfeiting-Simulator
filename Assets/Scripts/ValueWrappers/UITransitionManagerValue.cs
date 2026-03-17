using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UITransitionManagerSO", menuName = "Value Wrapper/Scripts/UITransitionManager")]
public class UITransitionManagerValue : ValueWrapper<UITransitionManager>
{
    private readonly List<UITransitionElement> pendingElements = new();

    public void Register(UITransitionElement element)
    {
        if (Value != null)
        {
            Value.Register(element);
        }
        else
        {
            if (!pendingElements.Contains(element))
            {
                pendingElements.Add(element);
            }
        }
    }

    public void Unregister(UITransitionElement element)
    {
        if (Value != null)
        {
            Value.Unregister(element);
        }
        else
        {
            if (pendingElements.Contains(element))
            {
                pendingElements.Remove(element);
            }
        }
    }

    public void ResolvePending()
    {
        if (Value == null) return;

        foreach (var uiTransitionElement in pendingElements)
        {
            if (uiTransitionElement != null)
            {
                Value.Register(uiTransitionElement);
            }
        }

        pendingElements.Clear();
    }
}
