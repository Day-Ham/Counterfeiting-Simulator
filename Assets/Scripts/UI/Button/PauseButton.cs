using DaeHanKim.ThisIsTotallyADollar.Core;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameManagerValue _gameManagerValue;
    [SerializeField] private Button _button;
    
    private GameManager _gameManager  => _gameManagerValue.Value;
    
    private void Awake()
    {
        _button.onClick.AddListener(PauseGame);
    }

    private void PauseGame()
    {
        _gameManager.PauseGame();
    }
}
