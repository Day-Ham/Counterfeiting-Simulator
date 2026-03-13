using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public Button StartButton;
    public SingleSceneReference SceneToGo;

    private void Awake()
    {
        StartButton.onClick.AddListener(GoToScene);
    }

    private void GoToScene()
    {
        string sceneNameToGo = SceneToGo.sceneName;

        SceneManager.LoadScene(sceneNameToGo);
    }
}
