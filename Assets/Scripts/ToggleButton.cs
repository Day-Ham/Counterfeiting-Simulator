using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private Button _toggleButton;
    [SerializeField] private GameObjectValue[] _listGameObject;

    private bool _isToggled;

    private void Awake()
    {
        _toggleButton.onClick.AddListener(ApplyToggle);
    }

    public void ApplyToggle()
    {
        _isToggled = !_isToggled;
        
        foreach (var value in _listGameObject)
        {
            if (value != null && value.Value != null)
            {
                value.Value.SetActive(_isToggled);
            }
        }
    }
}
