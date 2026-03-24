using System.Collections.Generic;
using UnityEngine;

public class UIStateController : MonoBehaviour
{
    [SerializeField] private List<GameObjectValue> UIElementsToDisable;

    private void OnEnable()
    {
        GameState.OnGameFinished += HandleGameFinished;
        GameState.OnGameStarted += HandleGameStarted;
        
        ApplyCurrentState();
    }

    private void OnDisable()
    {
        GameState.OnGameFinished -= HandleGameFinished;
        GameState.OnGameStarted -= HandleGameStarted;
    }

    private void HandleGameStarted()
    {
        SetUIActive(false);
    }

    private void HandleGameFinished()
    {
        SetUIActive(true);
    }

    private void SetUIActive(bool isActive)
    {
        foreach (var uiGameObjectValue in UIElementsToDisable)
        {
            if (uiGameObjectValue.Value)
            {
                uiGameObjectValue.Value.SetActive(isActive);
            }
        }
    }
    
    private void ApplyCurrentState()
    {
        SetUIActive(GameState.GameFinished);
    }
    
}
