using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private CallbackEvent playTransitionEvent;

    [Header("Buttons")]
    public Button button;

    [Header("Scene")]
    public SingleSceneReference sceneToGo;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        playTransitionEvent?.Raise(() =>
        {
            SceneManagerUtility.LoadScene(sceneToGo);
        });
    }
}
