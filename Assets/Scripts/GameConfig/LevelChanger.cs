using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEvent onRetryLevelEvent;
    [SerializeField] private VoidEvent onNextLevelEvent;
    [SerializeField] private VoidEvent sceneChangerEvent;
    [SerializeField] private VoidEvent onShowNextLevelButtonEvent;
    
    [Header("Transition Events")]
    [SerializeField] private VoidEvent playTransitionEvent;
    [SerializeField] private VoidEvent onTransitionFinishedEvent;

    [Header("References")] 
    [SerializeField] private LevelConfigListValue levelConfigListValue;
    [SerializeField] private IntValue currentLevelIndexValue;
    
    [SerializeField] private SingleSceneReference mainMenuScene;
    
    private bool _pendingNextLevel;
    private bool _pendingPrevLevel;
    private bool _pendingReset;
    private bool _pendingMainMenu;
    
    private void OnEnable()
    {
        sceneChangerEvent.Register(ShowNextButton);
        onRetryLevelEvent.Register(ResetLevel);
        onNextLevelEvent.Register(NextLevel);
        
        onTransitionFinishedEvent.Register(OnTransitionFinished);
    }

    private void OnDisable()
    {
        sceneChangerEvent.Unregister(ShowNextButton);
        onRetryLevelEvent.Unregister(ResetLevel);
        onNextLevelEvent.Unregister(NextLevel);
        
        onTransitionFinishedEvent.Unregister(OnTransitionFinished);
    }
    
    private void NextLevel()
    {
        _pendingNextLevel = true;
        playTransitionEvent.Raise();
    }
    
    private void PrevLevel()
    {
        if (currentLevelIndexValue.Value <= 0) return;

        _pendingPrevLevel = true;
        playTransitionEvent.Raise();
    }
    
    private void ResetLevel()
    {
        _pendingReset = true;
        playTransitionEvent.Raise();
    }
    
    private void LoadMainMenu()
    {
        _pendingMainMenu = true;
        playTransitionEvent.Raise();
    }
    
    private void OnTransitionFinished()
    {
        if (_pendingNextLevel) HandleNextLevel();
        if (_pendingPrevLevel) HandlePrevLevel();
        if (_pendingReset) HandleReset();
        if (_pendingMainMenu) HandleMainMenu();
    }
    
    private void ShowNextButton()
    {
        onShowNextLevelButtonEvent?.Raise();
    }
    
    private void HandleNextLevel()
    {
        _pendingNextLevel = false;

        bool isLastLevel = currentLevelIndexValue.Value >= levelConfigListValue.Value.Count - 1;

        if (isLastLevel)
        {
            LoadMainMenu();
            return;
        }

        currentLevelIndexValue.SetValue(currentLevelIndexValue.Value + 1);
        SceneManagerUtility.ReloadCurrentScene();
    }
    
    private void HandlePrevLevel()
    {
        _pendingPrevLevel = false;

        currentLevelIndexValue.SetValue(currentLevelIndexValue.Value - 1);
        SceneManagerUtility.ReloadCurrentScene();
    }
    
    private void HandleReset()
    {
        _pendingReset = false;

        currentLevelIndexValue.ForceNotify();
        SceneManagerUtility.ReloadCurrentScene();
    }
    
    private void HandleMainMenu()
    {
        _pendingMainMenu = false;

        if (!mainMenuScene)
        {
            Debug.LogWarning("Main Menu Scene is not assigned!");
            return;
        }

        SceneManagerUtility.LoadScene(mainMenuScene);
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftBracket))
        {
            PrevLevel();
        }
        
        if (Input.GetKey(KeyCode.RightBracket))
        {
            NextLevel();
        } 
    }
}
