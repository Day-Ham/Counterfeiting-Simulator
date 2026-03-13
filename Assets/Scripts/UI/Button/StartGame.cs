using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button StartButton;
    public SingleSceneReference SceneToGo;
    public SingleSceneReference SceneToUnload;

    private void Awake()
    {
        StartButton.onClick.AddListener(GoToGame);
    }

    private void GoToGame()
    {
        string sceneToGoName = SceneToGo.sceneName;
        string sceneToUnload = SceneToUnload.sceneName;
        
        SceneFlowManager.Instance.LoadScene(sceneToGoName);
        SceneManager.UnloadSceneAsync(sceneToUnload); 
    }
}
