using UnityEngine;
using UnityEngine.UI;

public class StartCompare : MonoBehaviour
{
    [SerializeField] private Button _compareButton;
    [SerializeField] private VoidEvent _startCompareEvent;
    
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
        _compareButton.onClick.AddListener(RaiseCompareEvent);
    }

    private void RaiseCompareEvent()
    {
        if (GameState.IsGameFinished) return;
        _startCompareEvent.Raise();
    }
    
    private void DisableInteraction()
    {
        _compareButton.interactable = false;
    }
}
