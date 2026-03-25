using UnityEngine;

public class PauseUIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;

    private void OnEnable()
    {
        GameState.OnGamePaused += ShowPauseUI;
        GameState.OnGameResumed += HidePauseUI;

        ApplyCurrentState();
    }

    private void OnDisable()
    {
        GameState.OnGamePaused -= ShowPauseUI;
        GameState.OnGameResumed -= HidePauseUI;
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        if (GameState.IsGamePaused)
        {
            GameState.ResumeGame();
        }
        else
        {
            GameState.PauseGame();
        }
    }

    private void ShowPauseUI()
    {
        pauseUI.SetActive(true);
    }

    private void HidePauseUI()
    {
        pauseUI.SetActive(false);
    }

    private void ApplyCurrentState()
    {
        pauseUI.SetActive(GameState.IsGamePaused);
    }
}
