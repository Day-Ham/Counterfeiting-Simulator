using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private VoidEvent SceneChangerEvent;
    [SerializeField] private GameObjectValue _circleTransition;
    [SerializeField] private GameObjectValue _nextButtonValue;
    public Ease EaseTween;
    
    private GameObject _nextButtonUI;
    private GameObject _circleTransitionUI;
    
    private void OnEnable()
    {
        SceneChangerEvent.OnRaised += ShowNextButton;
    }

    private void OnDisable()
    {
        SceneChangerEvent.OnRaised -= ShowNextButton;
    }

    private void Start()
    {
        _nextButtonUI = _nextButtonValue.Value;
        _circleTransitionUI = _circleTransition.Value;
        
        _circleTransitionUI.transform.DOScale(Vector3.zero, 1f);
        _nextButtonUI.transform.DOScale(Vector3.zero, 0f);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void ResetScene()
    {
        _circleTransitionUI.transform.DOScale(Vector3.one* 25f, 1f).OnComplete(
            () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });;
        
    }
    
    public void NextLevel()
    {
        _circleTransitionUI.transform.DOScale(Vector3.one* 25f, 1f).OnComplete(
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
        _circleTransitionUI.transform.DOScale(Vector3.one* 25f, 1f).OnComplete(
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
