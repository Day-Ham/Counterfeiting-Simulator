using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public Button StartButton;
    public SingleSceneReference SceneToGo;
    public TransitionControllerValue TransitionController;

    private void Awake()
    {
        StartButton.onClick.AddListener(GoToScene);
    }

    public void GoToScene()
    {
        SceneManagerUtility.LoadScene(SceneToGo, TransitionController?.Value);
    }
}
