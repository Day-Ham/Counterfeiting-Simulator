using System;
using System.Collections.Generic;
using UnityEngine;

public class UITransitionManager : MonoBehaviour
{
    [SerializeField] private UITransitionManagerValue managerValue;

    private readonly List<UITransitionElement> _uiTransitionElements = new();
    
    private int _completedCount;
    private Action _onAllComplete;

    private void OnEnable()
    {
        managerValue.Value = this;
        managerValue.ResolvePending();
    }

    public void Register(UITransitionElement element)
    {
        if (element == null) return;

        if (!_uiTransitionElements.Contains(element))
        {
            _uiTransitionElements.Add(element);
        }
    }

    public void Unregister(UITransitionElement element)
    {
        if (_uiTransitionElements.Contains(element))
        {
            _uiTransitionElements.Remove(element);
        }
    }

    public void MoveAllOut(Action onComplete = null)
    {
        _completedCount = 0;
        _onAllComplete = onComplete;

        if (_uiTransitionElements.Count == 0)
        {
            _onAllComplete?.Invoke();
            return;
        }

        foreach (var element in _uiTransitionElements)
        {
            element.OnMoveOutComplete += HandleElementDone;
            element.MoveOut();
        }
    }

    private void HandleElementDone()
    {
        _completedCount++;

        if (_completedCount < _uiTransitionElements.Count) return;
        
        foreach (var element in _uiTransitionElements)
        {
            element.OnMoveOutComplete -= HandleElementDone;
        }

        _onAllComplete?.Invoke();
    }

    public void MoveAllIn()
    {
        foreach (var uiTransitionElement in _uiTransitionElements)
        {
            uiTransitionElement.MoveIn();
        }
    }
}
