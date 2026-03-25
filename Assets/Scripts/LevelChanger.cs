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
    //[SerializeField] private GameObjectValue nextButtonValue;
    [SerializeField] private SingleSceneReference mainMenuScene;

    [Header("Tween Settings Next Button")]
    public Ease easeTween = Ease.OutBounce;
    
    private GameObject _nextButtonUI;
    private Tween _breathingTween;
    
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

    private void Start()
    {
        //_nextButtonUI = nextButtonValue.Value;
        //_nextButtonUI.transform.DOScale(Vector3.zero, 0f);
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
        /*_breathingTween?.Kill();

        // Scale in first
        _nextButtonUI.transform.DOScale(Vector3.one * .3f, .5f)
            .SetEase(easeTween)
            .OnComplete(() =>
            {
                // Start breathing loop
                _breathingTween = _nextButtonUI.transform.DOScale(Vector3.one * 0.35f, 0.8f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });*/
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
