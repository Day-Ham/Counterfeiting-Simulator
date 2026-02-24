using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    public SceneChangerValue SceneChangerValue;
    [SerializeField] private VoidEvent _sceneChangerEvent;
    [SerializeField] private GameObjectValue _circleTransition;
    [SerializeField] private GameObjectValue _nextButtonValue;
    public Ease EaseTween;
    
    [Header("Scene Settings")]
    [SerializeField] private MultipleSceneReference _multipleSceneReference;
    
    private int _currentLevelIndex;
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
        SceneChangerValue.Value = this;
    }

    private void Start()
    {
        DetectCurrentScene();
        
        _nextButtonUI = _nextButtonValue.Value;
        
        CircleUI.SetActive(true);
        
        CircleUI.transform.DOScale(Vector3.zero, 1f);
        _nextButtonUI.transform.DOScale(Vector3.zero, 0f);
    }

    private void DetectCurrentScene()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        
        for (int i = 0; i < _multipleSceneReference.Levels.Count; i++)
        {
            if (_multipleSceneReference.Levels[i].sceneName != activeSceneName) continue;
            
            _currentLevelIndex = i;
            break;
        }
    }

    public void QuitApp()
    {
        Application.Quit();
    }
    
    private void LoadSceneAdditive(string targetScene, Action<Scene, Scene> onLoaded)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != targetScene) return;

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.SetActiveScene(scene);

            // Callback with old and new scenes
            onLoaded?.Invoke(scene, SceneManager.GetSceneByName(currentScene));
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(targetScene, LoadSceneMode.Additive);
    }

    public void ResetScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        CircleUI.transform.DOScale(Vector3.one * 25f, 1f).OnComplete(() =>
        {
            LoadSceneAdditive(currentScene, (newScene, oldScene) =>
            {
                if (currentScene != "Core_Scene")
                {
                    SceneManager.UnloadSceneAsync(oldScene);
                }
            });
        });
    }
    
    public void NextLevel()
    {
        if (_currentLevelIndex >= _multipleSceneReference.Levels.Count - 1) return;

        string currentScene = SceneManager.GetActiveScene().name;
        string nextScene = _multipleSceneReference.Levels[_currentLevelIndex + 1].sceneName;

        CircleUI.transform.DOScale(Vector3.one * 25f, 1f).OnComplete(() =>
        {
            LoadSceneAdditive(nextScene, (newScene, oldScene) =>
            {
                if (currentScene != "Core_Scene")
                {
                    SceneManager.UnloadSceneAsync(oldScene);
                }
                _currentLevelIndex++;
            });
        });
    }

    private void ShowNextButton()
    {
        _nextButtonUI.transform.DOScale(Vector3.one * .3f, .5f).SetEase(EaseTween);
    }
    
    private void PrevLevel()
    {
        if (_currentLevelIndex <= 0)
        {
            return;
        }
        
        CircleUI.transform.DOScale(Vector3.one * 25f, 1f).OnComplete(() =>
        {
            SceneManager.LoadScene(_multipleSceneReference.Levels[_currentLevelIndex - 1].sceneName);
        });
        
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
