using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class AuctionMechanic : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private VoidEvent beginBidEvent;
    [SerializeField] private BidEvent onBidRaisedEvent;
    [SerializeField] private VoidEvent onAuctionEnd;
    
    [Header("Save System")]
    [SerializeField] private CanvasDrawControllerValue canvasDrawControllerValue;
    [SerializeField] private AuctionResultRuntime auctionResultRuntime;
    [SerializeField] private AuctionSaveHandler auctionSaveHandler;
    [SerializeField] private StringValue paintingName;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI auctionText;
    [SerializeField] private TextMeshProUGUI increasedBidText;
    [SerializeField] private TextMeshProUGUI auctionPromptText;
    [SerializeField] private TextMeshProUGUI bidderNameText;

    [Header("NPC Bidders")]
    [SerializeField] private List<NPCBidder> npcBidders;
    [SerializeField] private Transform bidderUIParent;

    [Header("Auction State")]
    [SerializeField] private int price = 0;
    [SerializeField] private float noBidTimeout = 3f;
    
    private readonly List<NPCBidderRuntime> _activeBidders = new();
    private int _wantValue = 100;
    private bool _isAnimatingBid = false;
    private bool _isEnding = false;
    private float _timeSinceLastBid = 0f;

    private void OnEnable()
    {
        beginBidEvent.Register(BeginBidding);
    }

    private void OnDisable()
    {
        beginBidEvent.Unregister(BeginBidding);
    }

    private void Start()
    {
        InitializeBidders();
    }

    private void InitializeBidders()
    {
        ClearOldUI();
        SpawnBidders();
    }
    
    private void ClearOldUI()
    {
        _activeBidders.Clear();

        foreach (Transform child in bidderUIParent)
        {
            Destroy(child.gameObject);
        }
    }
    
    private void SpawnBidders()
    {
        foreach (var npc in npcBidders)
        {
            var runtime = new NPCBidderRuntime(npc);
            AttachUI(runtime, npc);
            _activeBidders.Add(runtime);
        }
    }
    
    private void AttachUI(NPCBidderRuntime runtime, NPCBidder npc)
    {
        if (npc.bidderUIPrefab == null) return;

        var gameObjectInstantiate = Instantiate(npc.bidderUIPrefab, bidderUIParent);
        var bidderUI = gameObjectInstantiate.GetComponent<BidderUIBinder>();

        if (bidderUI == null)
        {
            Debug.LogWarning("Missing BidderUIBinder on prefab");
            return;
        }

        bidderUI.Bind(runtime);
        runtime.bidderUIBinder = bidderUI;
    }

    private void BeginBidding()
    {
        price = AuctionUtility.GenerateStartingPrice();
        auctionText.SetText("$" + price.ToString("n0"));

        _wantValue = 100;
        _isEnding = false;

        StartCoroutine(Bidding());
        StartCoroutine(DebugBidTimer());
    }

    private IEnumerator Bidding()
    {
        while (!_isEnding)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));

            _timeSinceLastBid += 1f;

            if (_isAnimatingBid) continue;

            if (TryProcessBid())
            {
                _timeSinceLastBid = 0f;
            }

            if (!(_timeSinceLastBid >= noBidTimeout)) continue;
            
            if (TryEndAuction(true))
            {
                _isEnding = true;
            }
        }
    }
    
    private IEnumerator DebugBidTimer()
    {
        while (!_isEnding)
        {
            Debug.Log($"[Timer] {_timeSinceLastBid:F1}s");
            yield return new WaitForSeconds(1f);
        }
    }

    private bool TryProcessBid()
    {
        var result = AuctionAI.TryGetBid(_activeBidders, price);

        if (!result.Success || _isAnimatingBid)
        {
            return false;
        }

        result.Bidder.currentMoney -= result.BidAmount;

        StartCoroutine(HandleBidVisuals(result));
        return true;
    }

    private IEnumerator HandleBidVisuals(BidResultStruct result)
    {
        _isAnimatingBid = true;
        
        bidderNameText.SetText(result.Bidder.data.npcName);
        increasedBidText.SetText("+$" + result.BidAmount.ToString("n0"));
        
        onBidRaisedEvent?.Raise(result.Bidder);

        yield return StartCoroutine(SmoothIncrease(price, result.NewPrice));

        price = result.NewPrice;
        auctionText.SetText("$" + price.ToString("n0"));

        yield return new WaitForSeconds(0.5f);
        
        increasedBidText.SetText("");

        _wantValue = AuctionUtility.DecreaseWantValue(_wantValue);
        _isAnimatingBid = false;
    }

    private bool TryEndAuction(bool forceEnd = false)
    {
        if (!forceEnd && _wantValue > 0)
        {
            return false;
        }

        StartCoroutine(FinalCountdown());
        return true;
    }

    private IEnumerator SmoothIncrease(int start, int end)
    {
        int priceValue = start;
        
        Debug.Log($"[Auction] Counting up started: {start} → {end}");
        
        if (priceValue % 1000 == 0)
        {
            Debug.Log($"[Auction] Counting: {priceValue}");
        }

        while (priceValue < end)
        {
            priceValue += AuctionUtility.GetSmoothStep(priceValue,end);
            priceValue = Mathf.Min(priceValue, end);

            auctionText.SetText("$" + priceValue.ToString("n0"));
            yield return new WaitForSeconds(0.02f);
        }
        
        Debug.Log($"[Auction] Counting finished: {end}");
        
    }

    private IEnumerator FinalCountdown()
    {
        auctionPromptText.SetText("GOING ONCE");
        yield return new WaitForSeconds(2);

        auctionPromptText.SetText("GOING TWICE");
        yield return new WaitForSeconds(2);

        auctionPromptText.SetText("SOLD!");
        
        StoreAuctionResult();
        auctionSaveHandler.Save();
        
        yield return new WaitForSeconds(1);
        
        onAuctionEnd?.Raise();
    }
    
    private void StoreAuctionResult()
    {
        var canvasState = canvasDrawControllerValue.Value.MainCanvasState;

        byte[] pngData = ConvertRenderTextureToPNG(canvasState.LayersRenderTextures[0]);

        auctionResultRuntime.SetData(pngData, price, paintingName.Value);
    }
    
    private byte[] ConvertRenderTextureToPNG(RenderTexture renderTexture)
    {
        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRenderTexture;

        byte[] bytes = texture2D.EncodeToPNG();
        Destroy(texture2D);

        return bytes;
    }
}
