using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AuctionLoadHandler : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEvent autoScrollEvent;
    [SerializeField] private VoidEvent refreshGalleryEvent;
    
    [Header("Gallery")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject auctionItemPrefab;
    
    private void OnEnable()
    {
        refreshGalleryEvent.Register(RefreshGallery);
    }

    private void OnDisable()
    {
        refreshGalleryEvent.Unregister(RefreshGallery);
    }

    private void Start()
    {
        LoadAllAuctions();
    }
    
    private void RefreshGallery()
    {
        LoadAllAuctions();
    }
    
    private void LoadAllAuctions()
    {
        var settings = new ES3Settings(SaveFileUtility.SAVE_FILE_NAME);

        if (!ES3.KeyExists(SaveFileUtility.SAVE_FILE_HISTORY, settings))
        {
            Debug.LogWarning("[Load] No auctions found.");
            return;
        }
        
        ClearGallery();

        var history = ES3.Load<List<AuctionSavedData>>(SaveFileUtility.SAVE_FILE_HISTORY, settings);
        
        ES3.Save(SaveFileUtility.SAVE_FILE_HISTORY, history, settings);

        foreach (var data in history)
        {
            CreateItem(data);
        }

        RebuildLayout();

        TryTriggerAutoScroll(settings);
    }
    
    private void TryTriggerAutoScroll(ES3Settings settings)
    {
        if (!ShouldAutoScroll(settings)) return;

        Debug.Log("[Gallery] Auto Scroll Triggered (Event)");

        ES3.Save(SaveFileUtility.AUTO_SCROLL_FLAG_KEY, false, settings);
        autoScrollEvent?.Raise();
    }

    private bool ShouldAutoScroll(ES3Settings settings)
    {
        return ES3.KeyExists(SaveFileUtility.AUTO_SCROLL_FLAG_KEY, settings) && ES3.Load<bool>(SaveFileUtility.AUTO_SCROLL_FLAG_KEY, settings);
    }
    
    private void CreateItem(AuctionSavedData savedData)
    {
        var gameObjectInstantiate = Instantiate(auctionItemPrefab, contentParent);
        var item = gameObjectInstantiate.GetComponent<AuctionGalleryItem>();

        if (item)
        {
            item.SetData(savedData);
        }
    }
    
    private void ClearGallery()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
    }
    
    private void RebuildLayout()
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentParent);
    }
}
