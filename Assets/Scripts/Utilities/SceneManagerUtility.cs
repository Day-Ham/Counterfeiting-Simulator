using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagerUtility
{
    /// <summary>
    /// Loads a scene from a SingleSceneReference.
    /// </summary>
    public static void LoadScene(SingleSceneReference sceneReference)
    {
        if (!sceneReference)
        {
            Debug.LogWarning("SceneReference is null!");
            return;
        }

        SceneManager.LoadScene(sceneReference.SceneName);
    }

    /// <summary>
    /// Reloads the current active scene.
    /// </summary>
    public static void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
