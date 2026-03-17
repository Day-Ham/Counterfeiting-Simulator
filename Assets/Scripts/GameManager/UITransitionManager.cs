using System.Collections.Generic;
using UnityEngine;

public class UITransitionManager : MonoBehaviour
{
    [SerializeField] private UITransitionManagerValue managerValue;

    private readonly List<UITransitionElement> elements = new();

    private void OnEnable()
    {
        managerValue.Value = this;
        managerValue.ResolvePending();
    }

    public void Register(UITransitionElement element)
    {
        if (element == null) return;

        if (!elements.Contains(element))
            elements.Add(element);
    }

    public void Unregister(UITransitionElement element)
    {
        if (elements.Contains(element))
            elements.Remove(element);
    }

    public void MoveAllOut()
    {
        foreach (var e in elements)
            e.MoveOut();
    }

    public void MoveAllIn()
    {
        foreach (var e in elements)
            e.MoveIn();
    }
}
