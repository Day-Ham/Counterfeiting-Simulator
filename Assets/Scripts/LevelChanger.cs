using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [Header("References")]
    public LevelChangerValue levelChangerValue;
    [SerializeField] private LevelManagerValue _levelManagerValue;
    [SerializeField] private VoidEvent _sceneChangerEvent;
    [SerializeField] private GameObjectValue _nextButtonValue;
    [SerializeField] private TransitionControllerValue _transitionController;
    [SerializeField] private SingleSceneReference _mainMenuScene;

    [Header("Tween Settings Next Button")]
    public Ease EaseTween = Ease.OutBounce;

    private LevelManager LevelManager => _levelManagerValue.Value;
    private GameObject _nextButtonUI;
    private Tween _breathingTween;
    
    private void OnEnable() => _sceneChangerEvent.Register(ShowNextButton);
    private void OnDisable() => _sceneChangerEvent.Unregister(ShowNextButton);

    private void Awake()
    {
        levelChangerValue.Value = this;
    }

    private void Start()
    {
        _nextButtonUI = _nextButtonValue.Value;
        _nextButtonUI.transform.DOScale(Vector3.zero, 0f);
    }
    
    public void NextLevel()
    {
        bool isLastLevel = LevelManager.CurrentLevelIndex >= LevelManager.LevelCount - 1;

        _transitionController.Value.PlayCloseTransition(() =>
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

        _transitionController.Value.PlayCloseTransition(() =>
        {
            LevelManager.LoadPrevLevel();
        });
    }
    
    public void ResetLevel()
    {
        _transitionController.Value.PlayCloseTransition(() =>
        {
            LevelManager.ReloadLevel();
        });
    }

    private void ShowNextButton()
    {
        _breathingTween?.Kill();

        // Scale in first
        _nextButtonUI.transform.DOScale(Vector3.one * .3f, .5f)
            .SetEase(EaseTween)
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
        if (!_mainMenuScene)
        {
            Debug.LogWarning("Main Menu Scene is not assigned!");
            return;
        }

        SceneManager.LoadScene(_mainMenuScene.SceneName);
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
