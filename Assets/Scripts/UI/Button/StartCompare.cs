using UnityEngine;
using UnityEngine.UI;

public class StartCompare : MonoBehaviour
{
    [SerializeField] private Button compareButton;
    [SerializeField] private VoidEvent startCompareEvent;
    
    private void OnEnable()
    {
        GameState.OnGameFinished += DisableInteraction;
    }

    private void OnDisable()
    {
        GameState.OnGameFinished -= DisableInteraction;
    }

    private void Awake()
    {
        compareButton.onClick.AddListener(RaiseCompareEvent);
    }

    private void RaiseCompareEvent()
    {
        if (GameState.IsGameFinished) return;
        startCompareEvent.Raise();
    }
    
    private void DisableInteraction()
    {
        compareButton.interactable = false;
    }
}
