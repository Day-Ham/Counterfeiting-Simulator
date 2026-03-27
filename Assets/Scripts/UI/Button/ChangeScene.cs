using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEvent playTransitionEvent;
    [SerializeField] private VoidEvent onTransitionFinished;

    [Header("Buttons")]
    public Button button;

    [Header("Scene")]
    public SingleSceneReference sceneToGo;
    
    private bool _shouldLoadScene;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }
    
    private void OnEnable()
    {
        onTransitionFinished.Register(OnTransitionFinished);
    }

    private void OnDisable()
    {
        onTransitionFinished.Unregister(OnTransitionFinished);
    }

    private void OnButtonClicked()
    {
        _shouldLoadScene = true;
        playTransitionEvent.Raise();
    }
    
    private void OnTransitionFinished()
    {
        if (!_shouldLoadScene) return;

        _shouldLoadScene = false;
        SceneManagerUtility.LoadScene(sceneToGo);
    }
}
