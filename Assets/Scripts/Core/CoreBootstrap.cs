using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreBootstrap : MonoBehaviour
{
    public SingleSceneReference MainMenuScene;

    private void Start()
    {
        LoadScene(MainMenuScene.sceneName);
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
