using UnityEngine;

public class AuctionEndSceneController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VoidEvent auctionEndEvent;
    [SerializeField] private SingleSceneReference galleryScene;
    [SerializeField] private TransitionControllerValue transitionController;

    private void OnEnable()
    {
        auctionEndEvent.Register(HandleAuctionEnd);
    }

    private void OnDisable()
    {
        auctionEndEvent.Unregister(HandleAuctionEnd);
    }

    private void HandleAuctionEnd()
    {
        SceneManagerUtility.LoadScene(galleryScene, transitionController?.Value);
    }
}
