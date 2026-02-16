using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private VoidEvent SceneChangerEvent;
    [SerializeField] private GameObject _circleTransition;
    [SerializeField] private GameObject _nextButton;
    public Ease EaseTween;
    
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
        _circleTransition.SetActive(true);
        _circleTransition.transform.DOScale(Vector3.zero, 1f);
        _nextButton.transform.DOScale(Vector3.zero, 0f);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void ResetScene()
    {
        _circleTransition.transform.DOScale(Vector3.one* 25f, 1f).OnComplete(
            () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });;
        
    }
    
    public void NextLevel()
    {
        _circleTransition.transform.DOScale(Vector3.one* 25f, 1f).OnComplete(
            () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            });;
        
    }

    private void ShowNextButton()
    {
        _nextButton.transform.DOScale(Vector3.one * .3f, .5f).SetEase(EaseTween);
    }
    
    private void PrevLevel()
    {
        _circleTransition.transform.DOScale(Vector3.one* 25f, 1f).OnComplete(
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
