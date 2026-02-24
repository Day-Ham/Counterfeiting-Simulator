using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button StartButton;
    public MultipleSceneReference _multipleSceneReference;

    private void Awake()
    {
        StartButton.onClick.AddListener(GoToGame);
    }

    private void GoToGame()
    {
        if (_multipleSceneReference.Scenes.Count == 0)
        {
            Debug.LogError("No levels in LevelDatabase!");
            return;
        }

        string firstLevelName = _multipleSceneReference.Scenes[0].sceneName;

        // Ask SceneFlowManager to load the first level
        SceneFlowManager.Instance.LoadScene(firstLevelName);
        SceneManager.UnloadSceneAsync("MainMenu"); 
    }
}
