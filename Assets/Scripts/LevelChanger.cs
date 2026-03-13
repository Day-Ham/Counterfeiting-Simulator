using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelChanger : MonoBehaviour
{
    [Header("References")]
    public LevelChangerValue levelChangerValue;
    [SerializeField] private LevelManagerValue _levelManagerValue;
    [SerializeField] private VoidEvent _sceneChangerEvent;
    [SerializeField] private GameObjectValue _circleTransition;
    [SerializeField] private GameObjectValue _nextButtonValue;

    [Header("Tween Settings")]
    public Ease EaseTween = Ease.OutBounce;

    private LevelManager LevelManager => _levelManagerValue.Value;
    private GameObject _nextButtonUI;
    private GameObject CircleUI => _circleTransition.Value;
    
    private void OnEnable() => _sceneChangerEvent.Register(ShowNextButton);
    private void OnDisable() => _sceneChangerEvent.Unregister(ShowNextButton);

    private void Awake()
    {
        levelChangerValue.Value = this;
    }

    private void Start()
    {
        _nextButtonUI = _nextButtonValue.Value;
        
        CircleUI.SetActive(true);
        
        CircleUI.transform.DOScale(Vector3.zero, 1f);
        _nextButtonUI.transform.DOScale(Vector3.zero, 0f);
    }
    
    public void NextLevel()
    {
        if (LevelManager.CurrentLevelIndex >= LevelManager.LevelCount - 1) return;

        CircleUI.transform.DOScale(Vector3.one * 25f, 1f).OnComplete(() =>
        {
            LevelManager.LoadNextLevel();
        });
    }
    
    private void PrevLevel()
    {
        if (LevelManager.CurrentLevelIndex <= 0) return;

        CircleUI.transform.DOScale(Vector3.one * 25f, 1f).OnComplete(() =>
        {
            LevelManager.LoadPrevLevel();
        });
    }
    
    public void ResetLevel()
    {
        CircleUI.transform.DOScale(Vector3.one * 25f, 1f).OnComplete(() =>
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
