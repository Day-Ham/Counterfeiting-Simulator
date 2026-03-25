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
    [SerializeField] private CallbackEvent playTransitionEvent;

    [Header("References")] 
    [SerializeField] private LevelConfigListValue levelConfigListValue;
    [SerializeField] private IntValue currentLevelIndexValue;
    
    [SerializeField] private LevelChangerValue levelChangerValue;
    [SerializeField] private SingleSceneReference mainMenuScene;
    
    private void OnEnable()
    {
        sceneChangerEvent.Register(ShowNextButton);
        onRetryLevelEvent.Register(ResetLevel);
        onNextLevelEvent.Register(NextLevel);
    }

    private void OnDisable()
    {
        sceneChangerEvent.Unregister(ShowNextButton);
        onRetryLevelEvent.Unregister(ResetLevel);
        onNextLevelEvent.Unregister(NextLevel);
    }

    private void Awake()
    {
        levelChangerValue.Value = this;
    }
    
    private void NextLevel()
    {
        bool isLastLevel = currentLevelIndexValue.Value >= levelConfigListValue.Value.Count - 1;

        playTransitionEvent?.Raise(() =>
        {
            if (isLastLevel)
            {
                LoadMainMenu();
            }
            else
            {
                currentLevelIndexValue.SetValue(currentLevelIndexValue.Value + 1);
                SceneManagerUtility.ReloadCurrentScene();
            }
        });
    }
    
    private void PrevLevel()
    {
        if (currentLevelIndexValue.Value <= 0) return;

        playTransitionEvent?.Raise(() =>
        {
            currentLevelIndexValue.SetValue(currentLevelIndexValue.Value - 1);
            SceneManagerUtility.ReloadCurrentScene();
        });
    }
    
    private void ResetLevel()
    {
        playTransitionEvent?.Raise(() =>
        {
            currentLevelIndexValue.ForceNotify();
            SceneManagerUtility.ReloadCurrentScene();
        });
    }

    private void ShowNextButton()
    {
        onShowNextLevelButtonEvent?.Raise();
    }
    
    private void LoadMainMenu()
    {
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
