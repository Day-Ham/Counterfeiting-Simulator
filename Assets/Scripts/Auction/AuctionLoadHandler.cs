using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AuctionLoadHandler : MonoBehaviour
{
    [Header("Gallery")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject auctionItemPrefab;

    [Header("Events")]
    [SerializeField] private VoidEvent autoScrollEvent;

    private const string SaveFileName = "AuctionSave.es3";
    private const string AutoScrollFlagKey = "GalleryAutoScroll";

    private void Start()
    {
        LoadAllAuctions();
    }
    
    private void LoadAllAuctions()
    {
        var settings = new ES3Settings(SaveFileName);

        if (!ES3.KeyExists("Auction_History", settings))
        {
            Debug.LogWarning("[Load] No auctions found.");
            return;
        }

        var history = ES3.Load<List<AuctionSavedData>>("Auction_History", settings);

        foreach (var data in history) CreateItem(data);

        RebuildLayout();

        TryTriggerAutoScroll(settings);
    }
    
    private void TryTriggerAutoScroll(ES3Settings settings)
    {
        if (!ShouldAutoScroll(settings)) return;

        Debug.Log("[Gallery] Auto Scroll Triggered (Event)");

        ES3.Save(AutoScrollFlagKey, false, settings);
        autoScrollEvent?.Raise();
    }

    private bool ShouldAutoScroll(ES3Settings settings)
    {
        return ES3.KeyExists(AutoScrollFlagKey, settings) && ES3.Load<bool>(AutoScrollFlagKey, settings);
    }
    
    private void CreateItem(AuctionSavedData data)
    {
        var gameObjectInstantiate = Instantiate(auctionItemPrefab, contentParent);
        var item = gameObjectInstantiate.GetComponent<AuctionGalleryItem>();

        if (item != null)
        {
            item.SetData(data.drawingData, data.finalPrice, data.paintingName);
        }
    }
    
    private void RebuildLayout()
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentParent);
    }
}
