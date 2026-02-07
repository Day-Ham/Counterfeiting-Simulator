using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject CircleTransition;
    [SerializeField] private GameObject NextButton;
    public Ease EaseTween;

    private void Start()
    {
        CircleTransition.SetActive(true);
        CircleTransition.transform.DOScale(Vector3.zero, 1f);
        NextButton.transform.DOScale(Vector3.zero, 0f);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void ResetScene()
    {
        CircleTransition.transform.DOScale(Vector3.one* 25f, 1f).OnComplete(
            () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });;
        
    }
    
    public void NextLevel()
    {
        CircleTransition.transform.DOScale(Vector3.one* 25f, 1f).OnComplete(
            () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            });;
        
    }

    public void ShowNextButton()
    {
        NextButton.transform.DOScale(Vector3.one * .3f, .5f).SetEase(EaseTween);
    }
    
    public void PrevLevel()
    {
        CircleTransition.transform.DOScale(Vector3.one* 25f, 1f).OnComplete(
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
