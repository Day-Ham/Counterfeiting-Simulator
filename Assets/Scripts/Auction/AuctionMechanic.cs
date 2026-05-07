using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.UI;

public class AuctionMechanic : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private VoidEvent beginBidEvent;
    [SerializeField] private BidEvent onBidRaisedEvent;
    [SerializeField] private VoidEvent onAuctionEnd;
    [SerializeField] private VoidEvent skipAuctionEvent;
    [SerializeField] private AudioClipEvent audioClipEvent;
    
    [Header("Save System")]
    [SerializeField] private CanvasDrawControllerValue canvasDrawControllerValue;
    [SerializeField] private AuctionResultRuntime auctionResultRuntime;
    [SerializeField] private AuctionSaveHandler auctionSaveHandler;
    [SerializeField] private StringValue paintingName;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI auctionText;
    [SerializeField] private TextMeshProUGUI increasedBidText;
    [SerializeField] private TextMeshProUGUI auctionPromptText;
    [SerializeField] private RectTransform soldStamp;

    [Header("NPC Bidders")]
    [SerializeField] private List<NPCBidder> npcBidders;
    [SerializeField] private Transform bidderUIParent;

    [Header("Auction State")]
    [SerializeField] private int price = 0;
    [SerializeField] private float noBidTimeout = 3f;

    [Header("Audio")]
    [SerializeField] private AudioClipValue auctionEndSFX;

    
    private readonly List<NPCBidderRuntime> _activeBidders = new();
    private int _wantValue = 100;
    private bool _isAnimatingBid = false;
    private bool _isEnding = false;
    private float _timeSinceLastBid = 0f;
    private Vector2 _bidTextOriginalPos;
    private Image _soldStampImage;

    private void OnEnable()
    {
        beginBidEvent.Register(BeginBidding);
        skipAuctionEvent.Register(DelaySkipAuction);
    }

    private void OnDisable()
    {
        beginBidEvent.Unregister(BeginBidding);
        skipAuctionEvent.Unregister(DelaySkipAuction);
    }

    private void Start()
    {
        InitializeBidders();
        _bidTextOriginalPos = increasedBidText.rectTransform.anchoredPosition;
        _soldStampImage = soldStamp.GetComponent<Image>();
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
        auctionText.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        auctionText.SetText("$" + price.ToString("n0"));

        _wantValue = 100;
        _isEnding = false;

        StartCoroutine(Bidding());
        //StartCoroutine(DebugBidTimer());
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
        
        RectTransform bidRect = (RectTransform)increasedBidText.transform;
        bidRect.DOKill();  
        bidRect.anchoredPosition = _bidTextOriginalPos;
        increasedBidText.alpha = 1f;

        increasedBidText.SetText($"{result.Bidder.data.npcName}\n+${result.BidAmount:n0}");
        increasedBidText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, vibrato: 3);
        
        onBidRaisedEvent?.Raise(result.Bidder);

        yield return new WaitForSeconds(0.5f);

        float animDuration = 0.5f;
        bidRect.DOAnchorPos(auctionText.rectTransform.anchoredPosition, animDuration);
        increasedBidText.DOFade(0f, 0.5f);

        yield return new WaitForSeconds(animDuration);

        increasedBidText.SetText("");

        yield return StartCoroutine(SmoothIncrease(price, result.NewPrice));

        price = result.NewPrice;
        auctionText.SetText("$" + price.ToString("n0"));

        yield return new WaitForSeconds(0.5f);
        
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
        
        //Debug.Log($"[Auction] Counting up started: {start} → {end}");
        
        if (priceValue % 1000 == 0)
        {
            //Debug.Log($"[Auction] Counting: {priceValue}");
        }
        var originalColor = auctionText.color;

        auctionText.transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, vibrato: 5);
        auctionText.color = Color.green;

        while (priceValue < end)
        {
            priceValue += AuctionUtility.GetSmoothStep(priceValue,end);
            priceValue = Mathf.Min(priceValue, end);

            auctionText.SetText("$" + priceValue.ToString("n0"));
            yield return new WaitForSeconds(0.02f);
        }
        
        auctionText.color = originalColor;
        
        //Debug.Log($"[Auction] Counting finished: {end}");
    }

    private IEnumerator FinalCountdown()
    {
        ShowPromptText("GOING ONCE");
        yield return new WaitForSeconds(2);

        ShowPromptText("GOING TWICE");
        yield return new WaitForSeconds(2);
        ShowPromptText("SOLD");
        PlaySoldStamp();   

        audioClipEvent?.Raise(auctionEndSFX?.Value);

        yield return new WaitForSeconds(0.5f);

        StoreAuctionResult();
        auctionSaveHandler.Save();

        yield return new WaitForSeconds(1f);

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

    private void DelaySkipAuction()
    {
        if (_isEnding || price == 0) return;
        ShowPromptText("SOLD");
        PlaySoldStamp();
        audioClipEvent?.Raise(auctionEndSFX?.Value);
        StopAllCoroutines();
        StartCoroutine(SkipAuction());
    }

    private IEnumerator SkipAuction()
    {
        yield return new WaitForSeconds(1.5f);
        _isEnding = true;
        StoreAuctionResult();
        auctionSaveHandler.Save();
        onAuctionEnd?.Raise();
    }

    private void ShowPromptText(string text)
    {
        auctionPromptText.transform.localScale = Vector3.zero;
        auctionPromptText.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        auctionPromptText.SetText(text);
    }

    private void PlaySoldStamp()
    {
        _soldStampImage.color = new Color(1, 1, 1, 0);
        soldStamp.localScale = Vector3.one * 4f;
        soldStamp.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        _soldStampImage.DOFade(1f, 0.15f);
    }
}
