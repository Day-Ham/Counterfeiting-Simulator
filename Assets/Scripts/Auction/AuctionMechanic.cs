using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AuctionMechanic : MonoBehaviour
{
    [Header("Save System")]
    [SerializeField] private CanvasDrawControllerValue canvasDrawControllerValue;
    [SerializeField] private AuctionResultRuntime auctionResultRuntime;
    [SerializeField] private AuctionSaveHandler auctionSaveHandler;
    [SerializeField] private StringValue paintingName;
    
    [Header("Auction Value")] 
    [SerializeField] private AuctionMechanicValue auctionMechanicValue;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI auctionText;
    [SerializeField] private TextMeshProUGUI increasedBidText;
    [SerializeField] private TextMeshProUGUI auctionPromptText;
    [SerializeField] private TextMeshProUGUI bidderNameText;

    [Header("NPC Bidders")]
    [SerializeField] private List<NPCBidder> npcBidders;

    private readonly List<NPCBidderRuntime> _activeBidders = new();

    [Header("Auction State")]
    [SerializeField] private int price = 0;
    [SerializeField] private float noBidTimeout = 3f;
    
    private int _wantValue = 100;
    private bool _isAnimatingBid = false;
    private bool _isEnding = false;
    private float _timeSinceLastBid = 0f;

    private void Awake()
    {
        auctionMechanicValue.Value = this;
    }

    private void Start()
    {
        InitializeBidders();
    }

    private void InitializeBidders()
    {
        _activeBidders.Clear();

        foreach (var npcBidder in npcBidders)
        {
            _activeBidders.Add(new NPCBidderRuntime(npcBidder));
        }
    }

    public void BeginBidding()
    {
        price = AuctionUtility.GenerateStartingPrice();
        auctionText.SetText("$" + price.ToString("n0"));

        _wantValue = 100;

        StartCoroutine(Bidding());
        StartCoroutine(DebugBidTimer());
    }

    private IEnumerator Bidding()
    {
        bool inAuction = true;
        
        while (inAuction)
        {
            if (_isEnding) yield break;
            
            float waitTime = Random.Range(1f, 3f);
            yield return new WaitForSeconds(waitTime);

            _timeSinceLastBid += waitTime;

            if (_isAnimatingBid) continue;

            if (TryProcessBid())
            {
                _timeSinceLastBid = 0f;
                continue;
            }

            if (!(_timeSinceLastBid >= noBidTimeout)) continue;
            
            if (!TryEndAuction(true)) continue;
            
            _isEnding = true;
            inAuction = false;
        }
    }
    
    private IEnumerator DebugBidTimer()
    {
        while (!_isEnding)
        {
            Debug.Log($"[Timer] Time Since Last Bid: {_timeSinceLastBid:F1}s");
            yield return new WaitForSeconds(1f);
        }
    }

    private bool TryProcessBid()
    {
        if (_isAnimatingBid) return false; // skip if previous animation not finished
        
        var result = AuctionAI.TryGetBid(_activeBidders, price);

        if (!result.success)
        {
            return false;
        }

        result.bidder.CurrentMoney -= result.bidAmount;

        StartCoroutine(HandleBidVisuals(result));

        return true;
    }

    private IEnumerator HandleBidVisuals(BidResultStruct result)
    {
        _isAnimatingBid = true;
        
        bidderNameText.SetText(result.bidder.Data.NpcName);
        increasedBidText.SetText("+$" + result.bidAmount.ToString("n0"));

        yield return StartCoroutine(SmoothIncrease(price, result.newPrice));

        price = result.newPrice;
        auctionText.SetText("$" + price.ToString("n0"));

        yield return new WaitForSeconds(0.5f);
        increasedBidText.SetText("");

        _wantValue = AuctionUtility.DecreaseWantValue(_wantValue);

        _isAnimatingBid = false;
    }

    private bool TryEndAuction(bool forceEnd = false)
    {
        if (!forceEnd && _wantValue > 0) return false;

        StartCoroutine(FinalCountdown());
        return true;
    }

    private IEnumerator SmoothIncrease(int start, int end)
    {
        int tempPrice = start;
        
        Debug.Log($"[Auction] Counting up started: {start} → {end}");
        
        if (tempPrice % 1000 == 0)
        {
            Debug.Log($"[Auction] Counting: {tempPrice}");
        }

        while (tempPrice < end)
        {
            tempPrice += AuctionUtility.GetSmoothStep(tempPrice,end);
            tempPrice = Mathf.Min(tempPrice, end);

            auctionText.SetText("$" + tempPrice.ToString("n0"));
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
    }
    
    private void StoreAuctionResult()
    {
        var canvasState = canvasDrawControllerValue.Value.MainCanvasState;

        byte[] pngData = ConvertRenderTextureToPNG(canvasState.LayersRenderTextures[0]);

        auctionResultRuntime.SetData(pngData, price, paintingName.Value);
    }
    
    private byte[] ConvertRenderTextureToPNG(RenderTexture rt)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = currentRT;

        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);

        return bytes;
    }
}
