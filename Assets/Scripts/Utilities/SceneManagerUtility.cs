using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagerUtility
{
    /// <summary>
    /// Loads a scene with an optional transition.
    /// </summary>
    public static void LoadScene(string sceneName, TransitionController transition = null)
    {
        if (transition)
        {
            transition.PlayCloseTransition(() =>
            {
                SceneManager.LoadScene(sceneName);
            });
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    /// <summary>
    /// Loads a scene from a SingleSceneReference.
    /// </summary>
    public static void LoadScene(SingleSceneReference sceneReference, TransitionController transition = null)
    {
        if (!sceneReference)
        {
            Debug.LogWarning("SceneReference is null!");
            return;
        }

        LoadScene(sceneReference.SceneName, transition);
    }

    /// <summary>
    /// Reloads the current active scene.
    /// </summary>
    public static void ReloadCurrentScene(TransitionController transition = null)
    {
        LoadScene(SceneManager.GetActiveScene().name, transition);
    }
}
