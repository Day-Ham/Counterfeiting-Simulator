using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private AuctionSavedDataEvent selectedItemEvent;
    [SerializeField] private BoolEvent contextMenuToggle;
    [SerializeField] private VoidEvent refreshGalleryEvent;

    [Header("UI")]
    [SerializeField] private Button deleteButton;

    private AuctionSavedData _selectedData;

    private const string SaveFileName = "AuctionSave.es3";
    private const string SaveKey = "Auction_History";

    private void OnEnable()
    {
        deleteButton.onClick.AddListener(DeleteSelectedItem);
        selectedItemEvent.Register(SetSelectedItem);
    }

    private void OnDisable()
    {
        deleteButton.onClick.RemoveListener(DeleteSelectedItem);
        selectedItemEvent.Unregister(SetSelectedItem);
    }

    private void SetSelectedItem(AuctionSavedData data)
    {
        _selectedData = data;
    }

    private void DeleteSelectedItem()
    {
        if (_selectedData == null) return;

        ES3Settings settings = new ES3Settings(SaveFileName);

        if (!ES3.KeyExists(SaveKey, settings)) return;

        List<AuctionSavedData> history = ES3.Load<List<AuctionSavedData>>(SaveKey, settings);

        history.RemoveAll(auctionSavedData => auctionSavedData.paintingID == _selectedData.paintingID);

        ES3.Save(SaveKey, history, settings);

        Debug.Log($"[Delete] Removed: {_selectedData.paintingName}");

        refreshGalleryEvent?.Raise();
        contextMenuToggle.Raise(false);

        _selectedData = null;
    }
}
