using UnityEngine;

public class AuctionEndSceneController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VoidEvent auctionEndEvent;
    [SerializeField] private CallbackEvent transitionEvent;
    
    [Header("Scene")]
    [SerializeField] private SingleSceneReference galleryScene;

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
        transitionEvent?.Raise(() =>
        {
            SceneManagerUtility.LoadScene(galleryScene);
        });
    }
}
