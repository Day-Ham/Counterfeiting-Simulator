using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnPauseClicked);
    }

    private void OnPauseClicked()
    {
        GameState.PauseGame();
    }
}
