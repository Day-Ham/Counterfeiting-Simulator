using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AuctionLoadHandler : MonoBehaviour
{
    [Header("Gallery")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject auctionItemPrefab;
    
    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration;
    [SerializeField] private Ease autoScrollEase;

    private const string SaveFileName = "AuctionSave.es3";
    private const string AutoScrollFlagKey = "GalleryAutoScroll";
    
    private Tween _scrollTween;
    private Coroutine _scrollCoroutine;
    
    
    private void Start()
    {
        LoadAllAuctions();
    }
    
    // Load Saved Data (No Scroll)
    private void LoadAllAuctions()
    {
        ES3Settings settings = new ES3Settings(SaveFileName);

        if (!ES3.KeyExists("Auction_History", settings))
        {
            Debug.LogWarning("[Load] No auctions found.");
            return;
        }

        List<AuctionSavedData> history = ES3.Load<List<AuctionSavedData>>("Auction_History", settings);

        Debug.Log($"[Load] Loading {history.Count} auctions");

        foreach (var data in history)
        {
            CreateItem(data);
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentParent);
        
        bool shouldAutoScroll = false;

        if (ES3.KeyExists(AutoScrollFlagKey, settings))
        {
            shouldAutoScroll = ES3.Load<bool>(AutoScrollFlagKey, settings);
        }

        if (!shouldAutoScroll) return;
        
        Debug.Log("[Gallery] Coming from Auction → Auto Scroll");

        // IMPORTANT: reset immediately so it won't trigger again
        ES3.Save(AutoScrollFlagKey, false, settings);

        if (_scrollCoroutine != null)
        {
            StopCoroutine(_scrollCoroutine);
        }

        _scrollCoroutine = StartCoroutine(ScrollToLatest());
    }
    
    private IEnumerator ScrollToLatest()
    {
        SetScrollInteractable(false);
        
        yield return null;
        yield return new WaitForEndOfFrame();
        
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentParent);
        
        _scrollTween?.Kill();

        _scrollTween = DOTween.To(
                () => scrollRect.horizontalNormalizedPosition,
                x => scrollRect.horizontalNormalizedPosition = x,
                1f,
                tweenDuration
            )
            .SetEase(autoScrollEase);
        
        SetScrollInteractable(true);
    }
    
    private void CreateItem(AuctionSavedData data)
    {
        GameObject galleryImageUI = Instantiate(auctionItemPrefab, contentParent);
        AuctionGalleryItem galleryItem = galleryImageUI.GetComponent<AuctionGalleryItem>();

        if (galleryItem)
        {
            galleryItem.SetData(data.DrawingData, data.FinalPrice, data.PaintingName);
        }
    }
    
    private void SetScrollInteractable(bool value)
    {
        scrollRect.horizontal = value;
        scrollRect.inertia = value;
    }
}
