using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button StartButton;
    public SingleSceneReference Level1Scene;
    public SingleSceneReference MainMenuScene;

    private void Awake()
    {
        StartButton.onClick.AddListener(GoToGame);
    }

    private void GoToGame()
    {
        string level1SceneSceneName = Level1Scene.sceneName;
        string mainMenuSceneName = MainMenuScene.sceneName;
        
        SceneFlowManager.Instance.LoadScene(level1SceneSceneName);
        SceneManager.UnloadSceneAsync(mainMenuSceneName); 
    }
}
