using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button StartButton;
    public SingleSceneReference SceneToGo;
    public SingleSceneReference MainMenuScene;

    private void Awake()
    {
        StartButton.onClick.AddListener(GoToGame);
    }

    private void GoToGame()
    {
        string level1SceneSceneName = SceneToGo.sceneName;
        string mainMenuSceneName = MainMenuScene.sceneName;
        
        SceneFlowManager.Instance.LoadScene(level1SceneSceneName);
        SceneManager.UnloadSceneAsync(mainMenuSceneName); 
    }
}
