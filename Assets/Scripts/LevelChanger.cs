using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEvent onRetryLevel;
    [SerializeField] private VoidEvent onNextLevel;
    [SerializeField] private VoidEvent sceneChangerEvent;
    
    [Header("References")]
    [SerializeField] private LevelChangerValue levelChangerValue;
    [SerializeField] private LevelManagerValue levelManagerValue;
    [SerializeField] private GameObjectValue nextButtonValue;
    [SerializeField] private TransitionControllerValue transitionController;
    [SerializeField] private SingleSceneReference mainMenuScene;

    [Header("Tween Settings Next Button")]
    public Ease easeTween = Ease.OutBounce;
    
    private LevelManager LevelManager => levelManagerValue.Value;
    
    private GameObject _nextButtonUI;
    private Tween _breathingTween;
    
    private void OnEnable()
    {
        sceneChangerEvent.Register(ShowNextButton);
        onRetryLevel.Register(ResetLevel);
        onNextLevel.Register(NextLevel);
    }

    private void OnDisable()
    {
        sceneChangerEvent.Unregister(ShowNextButton);
        onRetryLevel.Unregister(ResetLevel);
        onNextLevel.Unregister(NextLevel);
    }

    private void Awake()
    {
        levelChangerValue.Value = this;
    }

    private void Start()
    {
        _nextButtonUI = nextButtonValue.Value;
        _nextButtonUI.transform.DOScale(Vector3.zero, 0f);
    }
    
    private void NextLevel()
    {
        bool isLastLevel = LevelManager.CurrentLevelIndex >= LevelManager.LevelCount - 1;

        transitionController.Value.PlayCloseTransition(() =>
        {
            if (isLastLevel)
            {
                LoadMainMenu();
            }
            else
            {
                LevelManager.LoadNextLevel();
            }
        });
    }
    
    private void PrevLevel()
    {
        if (LevelManager.CurrentLevelIndex <= 0) return;

        transitionController.Value.PlayCloseTransition(() =>
        {
            LevelManager.LoadPrevLevel();
        });
    }
    
    private void ResetLevel()
    {
        transitionController.Value.PlayCloseTransition(() =>
        {
            LevelManager.ReloadLevel();
        });
    }

    private void ShowNextButton()
    {
        _breathingTween?.Kill();

        // Scale in first
        _nextButtonUI.transform.DOScale(Vector3.one * .3f, .5f)
            .SetEase(easeTween)
            .OnComplete(() =>
            {
                // Start breathing loop
                _breathingTween = _nextButtonUI.transform.DOScale(Vector3.one * 0.35f, 0.8f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });
    }
    
    private void LoadMainMenu()
    {
        if (!mainMenuScene)
        {
            Debug.LogWarning("Main Menu Scene is not assigned!");
            return;
        }

        SceneManager.LoadScene(mainMenuScene.SceneName);
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
