using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBootstrap : MonoBehaviour
{
    private void Start()
    {
        if (!SceneManager.GetSceneByName("BGM_Scene").isLoaded)
        {
            SceneManager.LoadScene("BGM_Scene", LoadSceneMode.Additive);
        }
    }
}
