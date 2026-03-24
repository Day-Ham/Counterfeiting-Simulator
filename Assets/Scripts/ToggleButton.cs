using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private Button _toggleButton;
    [SerializeField] private GameObjectValue[] _listGameObject;

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
        _toggleButton.onClick.AddListener(ApplyToggle);
    }

    public void ApplyToggle()
    {
        if (GameState.IsGameFinished) return;
        
        _isToggled = !_isToggled;
        UpdateGameObjects();
    }

    private void UpdateGameObjects()
    {
        foreach (var value in _listGameObject)
        {
            if (value && value.Value)
            {
                value.Value.SetActive(_isToggled);
            }
        }
    }
    
    private void DisableInteraction()
    {
        _isToggled = false;
        _toggleButton.interactable = false;
        UpdateGameObjects();
    }
}
