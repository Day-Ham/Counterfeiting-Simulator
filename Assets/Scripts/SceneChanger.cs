using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    [Header("References")]
    public SceneChangerValue SceneChangerValue;
    [SerializeField] private LevelManagerValue _levelManagerValue;
    [SerializeField] private VoidEvent _sceneChangerEvent;
    [SerializeField] private GameObjectValue _circleTransition;
    [SerializeField] private GameObjectValue _nextButtonValue;

    [Header("Tween Settings")]
    public Ease EaseTween = Ease.OutBounce;

    private LevelManager LevelManager => _levelManagerValue.Value;
    private GameObject _nextButtonUI;
    private GameObject CircleUI => _circleTransition.Value;
    private bool isTransitioning;
    
    private void OnEnable() => _sceneChangerEvent.Register(ShowNextButton);
    private void OnDisable() => _sceneChangerEvent.Unregister(ShowNextButton);

    private void Awake()
    {
        SceneChangerValue.Value = this;
    }

    private void Start()
    {
        _nextButtonUI = _nextButtonValue.Value;
        
        //CircleUI.SetActive(true);
        
        CircleUI.transform.DOScale(Vector3.zero, 1f);
        _nextButtonUI.transform.DOScale(Vector3.zero, 0f);
        OnLevelLoaded();
    }
    
    public void NextLevel()
    {
        if (LevelManager.CurrentLevelIndex >= LevelManager.LevelCount - 1)
        {
            OnLevelLoaded(); // make sure we reset
            return;
        }

        CircleUI.transform.DOScale(Vector3.one * 25f, 1f).OnComplete(() =>
        {
            LevelManager.LoadNextLevel();
            OnLevelLoaded();
        });
    }
    
    private void PrevLevel()
    {
        if (LevelManager.CurrentLevelIndex <= 0)
        {
            OnLevelLoaded(); // make sure we reset
            return;
        }

        CircleUI.transform.DOScale(Vector3.one * 25f, 1f).OnComplete(() =>
        {
            LevelManager.LoadPrevLevel();
            OnLevelLoaded();
        });
    }
    
    public void ResetLevel()
    {
        CircleUI.transform.DOScale(Vector3.one * 25f, 1f).OnComplete(() =>
        {
            LevelManager.ReloadLevel();
            OnLevelLoaded();
        });
    }

    private void ShowNextButton()
    {
        _nextButtonUI.transform.DOScale(Vector3.one * .3f, .5f).SetEase(EaseTween);
    }
    
    private void Update()
    {
        if (isTransitioning) return;

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            isTransitioning = true;
            PrevLevel();
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            isTransitioning = true;
            NextLevel();
        }
    }
    
    private void OnLevelLoaded()
    {
        isTransitioning = false;
    }
}
