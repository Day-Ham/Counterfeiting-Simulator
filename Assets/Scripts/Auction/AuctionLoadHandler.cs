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
    
    private Tween _scrollTween;
    private Coroutine _scrollCoroutine;
    
    private int _lastLoadedCount = 0;

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
            Debug.LogWarning("[AuctionLoadHandler] No saved auctions found.");
            return;
        }

        List<AuctionSavedData> history = ES3.Load<List<AuctionSavedData>>("Auction_History", settings);
        Debug.Log($"[AuctionLoadHandler] Loading {history.Count} auctions");

        foreach (var auctionSavedData in history)
        {
            CreateItem(auctionSavedData);
        }
        
        if (history.Count > _lastLoadedCount)
        {
            int newSavedItems = history.Count - _lastLoadedCount;

            for (int i = history.Count - newSavedItems; i < history.Count; i++)
            {
                AddNewAuction(history[i]);
            }
        }

        _lastLoadedCount = history.Count;
    }

    private void AddNewAuction(AuctionSavedData data)
    {
        CreateItem(data);

        if (_scrollCoroutine != null)
        {
            StopCoroutine(_scrollCoroutine);
        }

        _scrollCoroutine = StartCoroutine(ScrollToLatest());
    }
    
    private void CreateItem(AuctionSavedData data)
    {
        GameObject galleryImageUI = Instantiate(auctionItemPrefab, contentParent);
        AuctionGalleryItem galleryItem = galleryImageUI.GetComponent<AuctionGalleryItem>();

        if (galleryItem != null)
        {
            galleryItem.SetData(data.DrawingData, data.FinalPrice, data.PaintingName);
        }
    }
    
    private IEnumerator ScrollToLatest()
    {
        SetScrollInteractable(false);
        
        yield return new WaitForSeconds(1.5f);
        
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
    
    private void SetScrollInteractable(bool value)
    {
        scrollRect.horizontal = value;
        scrollRect.vertical = value;
        scrollRect.inertia = value;
    }
}
