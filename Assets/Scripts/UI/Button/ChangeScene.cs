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
    
    private void OnEnable()
    {
        playTransitionEvent?.Register((onComplete) => GoToScene());
    }

    private void OnDisable()
    {
        playTransitionEvent?.Unregister((onComplete) => GoToScene());
    }

    private void Awake()
    {
        button.onClick.AddListener(GoToScene);
    }

    private void GoToScene()
    {
        playTransitionEvent?.Raise();
        
        SceneManagerUtility.LoadScene(sceneToGo);
    }
}
