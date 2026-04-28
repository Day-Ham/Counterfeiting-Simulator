using System.Collections.Generic;
using UnityEngine;

public class UIPanelNavigator : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private RectTransformEvent showPanelEvent;
    [SerializeField] private VoidEvent showPreviousPanelEvent;
    [SerializeField] private RescaleTweenUnitScriptableObject rescaleTween;

    [Header("Initial Panel")]
    [SerializeField] private RectTransform initialPanel;

    private RectTransform _currentPanel;
    private bool _isTransitioning;
    private Stack<RectTransform> _history = new Stack<RectTransform>();

    public bool IsTransitioning => _isTransitioning;

    private void OnEnable()
    {
        showPanelEvent.Register(NavigateTo);
        showPreviousPanelEvent.Register(NavigateBack);
    }

    private void OnDisable()
    {
        showPanelEvent.Unregister(NavigateTo);
        showPreviousPanelEvent.Unregister(NavigateBack);
    }

    private void Start()
    {
        _currentPanel = initialPanel;
        _currentPanel.gameObject.SetActive(true);
    }

    private void NavigateBack()
    {
        if (_isTransitioning || _history.Count == 0) return;
        RectTransform prev = _currentPanel;
        _currentPanel = _history.Pop();
        Transition(prev, _currentPanel);
    }

    private void NavigateTo(RectTransform nextPanel)
    {
        if (_isTransitioning || _currentPanel == nextPanel) return;
        RectTransform previousPanel = _currentPanel;
        _history.Push(previousPanel);
        _currentPanel = nextPanel;
        Transition(previousPanel, nextPanel);
    }

    private void Transition(RectTransform from, RectTransform to)
    {
        _isTransitioning = true;
        rescaleTween.Collapse(from, () =>
        {
            from.gameObject.SetActive(false);
            to.gameObject.SetActive(true);
            rescaleTween.Expand(to, () => _isTransitioning = false);
        });
    }

    public bool TryNavigateBack()
    {
        if (_isTransitioning || _history.Count == 0) return false;
        NavigateBack();
        return true;
    }
}