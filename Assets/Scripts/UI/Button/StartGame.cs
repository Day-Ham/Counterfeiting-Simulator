using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button StartButton;
    public SingleSceneReference SceneToGo;
    public SingleSceneReference SceneToUnload;
    public GameObjectValue CircleTransition;
    
    private GameObject CircleUI => CircleTransition.Value;

    private void Awake()
    {
        StartButton.onClick.AddListener(GoToGame);
    }

    private void GoToGame()
    {
        // Make sure Circle UI is visible and reset its scale
        CircleUI.SetActive(true);
        CircleUI.transform.localScale = Vector3.zero;
        
        CircleUI.transform.DOScale(Vector3.one * 25f, 1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                SceneFlowManager.Instance.LoadScene(SceneToGo.sceneName);
                SceneManager.UnloadSceneAsync(SceneToUnload.sceneName);
            });
    }
}
