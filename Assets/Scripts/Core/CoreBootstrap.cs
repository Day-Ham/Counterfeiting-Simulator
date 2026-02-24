using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreBootstrap : MonoBehaviour
{
    [SerializeField] private string firstSceneToLoad = "MainMenu";

    private void Start()
    {
        LoadScene(firstSceneToLoad);
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
