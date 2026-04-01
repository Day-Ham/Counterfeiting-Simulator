using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private Button toggleButton;
    [SerializeField] private BoolEvent boolEvent;

    private bool _isToggled;

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
        toggleButton.onClick.AddListener(ApplyToggle);
    }

    private void ApplyToggle()
    {
        if (GameState.IsGameFinished) return;

        _isToggled = !_isToggled;
        boolEvent?.Raise(_isToggled);
    }

    private void DisableInteraction()
    {
        _isToggled = false;
        toggleButton.interactable = false;
        boolEvent?.Raise(_isToggled);
    }
}
