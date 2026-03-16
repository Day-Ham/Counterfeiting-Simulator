using UnityEngine;
using DG.Tweening;

public class LevelChanger : MonoBehaviour
{
    [Header("References")]
    public LevelChangerValue levelChangerValue;
    [SerializeField] private LevelManagerValue _levelManagerValue;
    [SerializeField] private VoidEvent _sceneChangerEvent;
    [SerializeField] private GameObjectValue _nextButtonValue;
    [SerializeField] private TransitionControllerValue _transitionController;

    [Header("Tween Settings Next Button")]
    public Ease EaseTween = Ease.OutBounce;

    private LevelManager LevelManager => _levelManagerValue.Value;
    private GameObject _nextButtonUI;
    
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
        if (LevelManager.CurrentLevelIndex >= LevelManager.LevelCount - 1) return;

        _transitionController.Value.PlayCloseTransition(() =>
        {
            LevelManager.LoadNextLevel();
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
        _nextButtonUI.transform.DOScale(Vector3.one * .3f, .5f).SetEase(EaseTween);
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
