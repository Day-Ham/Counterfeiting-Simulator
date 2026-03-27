using UnityEngine;

public class AuctionEndSceneController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VoidEvent auctionEndEvent;
    [SerializeField] private VoidEvent playTransitionEvent;
    [SerializeField] private VoidEvent onTransitionFinishedEvent;
    
    [Header("Scene")]
    [SerializeField] private SingleSceneReference galleryScene;
    
    private bool _loadGallery;

    private void OnEnable()
    {
        auctionEndEvent.Register(HandleAuctionEnd);
        onTransitionFinishedEvent.Register(OnTransitionFinished);
    }

    private void OnDisable()
    {
        auctionEndEvent.Unregister(HandleAuctionEnd);
        onTransitionFinishedEvent.Unregister(OnTransitionFinished);
    }

    private void HandleAuctionEnd()
    {
        _loadGallery = true;
        playTransitionEvent.Raise();
    }

    private void OnTransitionFinished()
    {
        if (!_loadGallery) return;

        _loadGallery = false;
        SceneManagerUtility.LoadScene(galleryScene);
    }
}
