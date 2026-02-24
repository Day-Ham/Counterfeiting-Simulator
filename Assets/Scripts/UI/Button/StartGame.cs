using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button StartButton;
    public LevelDatabase LevelDatabase;

    private void Awake()
    {
        StartButton.onClick.AddListener(GoToGame);
    }

    private void GoToGame()
    {
        if (LevelDatabase.Levels.Count == 0)
        {
            Debug.LogError("No levels in LevelDatabase!");
            return;
        }

        string firstLevelName = LevelDatabase.Levels[0].sceneName;

        // Ask SceneFlowManager to load the first level
        SceneFlowManager.Instance.LoadScene(firstLevelName);
        SceneManager.UnloadSceneAsync("MainMenu"); 
    }
}
