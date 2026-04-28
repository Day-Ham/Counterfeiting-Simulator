using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowPanelButton : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private RectTransformEvent showPanelEvent;

    [Header("Panel to show")]
    [SerializeField] private RectTransform panelToShow;
    
    [Header("Button")]
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        showPanelEvent.Raise(panelToShow);
    }
}
