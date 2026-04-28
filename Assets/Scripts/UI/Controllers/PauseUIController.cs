using System;
using UnityEngine;

public class PauseUIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvasGroup;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private TweenAnimationUnitScriptable tweenAnimationUnitScriptable;
    [SerializeField] private UIPanelNavigator panelNavigator;

    private void OnEnable()
    {
        GameState.OnGamePaused += ShowPauseUI;
        GameState.OnGameResumed += HidePauseUI;

        pauseUI.SetActive(false);
    }

    private void OnDisable()
    {
        GameState.OnGamePaused -= ShowPauseUI;
        GameState.OnGameResumed -= HidePauseUI;
    }

    private void Start()
    {
        //GameState.ResumeGame();
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (panelNavigator.IsTransitioning) return;
        
        if (!GameState.IsGamePaused)
        {
            GameState.PauseGame();
            return;
        }

        if (!panelNavigator.TryNavigateBack())
            GameState.ResumeGame();
    }

    private void ShowPauseUI()
    {
        pauseUI.SetActive(true);
        tweenAnimationUnitScriptable?.Play(pauseCanvasGroup.GetComponent<RectTransform>());
    }
    private void HidePauseUI()
    {
        tweenAnimationUnitScriptable?.PlayReverse(pauseCanvasGroup.GetComponent<RectTransform>()
        , () => pauseUI.SetActive(false));
    }
}
