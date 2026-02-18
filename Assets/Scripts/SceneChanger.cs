using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    public SceneChangerValue _sceneChangerValue;
    [SerializeField] private VoidEvent _sceneChangerEvent;
    [SerializeField] private GameObjectValue _circleTransition;
    [SerializeField] private GameObjectValue _nextButtonValue;
    public Ease EaseTween;
    
    private GameObject _nextButtonUI;
    
    private GameObject CircleUI => _circleTransition.Value;
    
    private void OnEnable()
    {
        _sceneChangerEvent.OnRaised += ShowNextButton;
    }

    private void OnDisable()
    {
        _sceneChangerEvent.OnRaised -= ShowNextButton;
    }

    private void Awake()
    {
        _sceneChangerValue.Value = this;
    }

    private void Start()
    {
        _nextButtonUI = _nextButtonValue.Value;
        
        CircleUI.SetActive(true);
        
        CircleUI.transform.DOScale(Vector3.zero, 1f);
        _nextButtonUI.transform.DOScale(Vector3.zero, 0f);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void ResetScene()
    {
        CircleUI.transform.DOScale(Vector3.one* 25f, 1f).OnComplete(
            () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });;
    }
    
    public void NextLevel()
    {
        CircleUI.transform.DOScale(Vector3.one* 25f, 1f).OnComplete(
            () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            });;
        
    }

    private void ShowNextButton()
    {
        _nextButtonUI.transform.DOScale(Vector3.one * .3f, .5f).SetEase(EaseTween);
    }
    
    private void PrevLevel()
    {
        CircleUI.transform.DOScale(Vector3.one* 25f, 1f).OnComplete(
            () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
            });;
        
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
