using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;

    private string _currentLoadedScene;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (!string.IsNullOrEmpty(_currentLoadedScene))
        {
            Scene prevScene = SceneManager.GetSceneByName(_currentLoadedScene);
            if (prevScene.isLoaded)
                yield return SceneManager.UnloadSceneAsync(_currentLoadedScene);
        }

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(newScene);

        _currentLoadedScene = sceneName;
    }
    
    public void ReloadCurrentScene()
    {
        if (string.IsNullOrEmpty(_currentLoadedScene)) return;

        StartCoroutine(ReloadSceneRoutine());
    }

    private IEnumerator ReloadSceneRoutine()
    {
        string sceneName = _currentLoadedScene;

        if (!string.IsNullOrEmpty(sceneName) && SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
        }

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(newScene);
    }
}
