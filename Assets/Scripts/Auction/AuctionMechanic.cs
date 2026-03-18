using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AuctionMechanic : MonoBehaviour
{
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
    private int _wantValue = 100;

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
    }

    private IEnumerator Bidding()
    {
        bool inAuction = true;

        while (inAuction)
        {
            yield return WaitForNextTurn();

            if (TryProcessBid()) continue;

            if (TryEndAuction())
            {
                inAuction = false;
            }
        }
    }

    private IEnumerator WaitForNextTurn()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
    }

    private bool TryProcessBid()
    {
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
        bidderNameText.SetText(result.bidder.Data.NpcName);
        increasedBidText.SetText("+$" + result.bidAmount.ToString("n0"));

        yield return StartCoroutine(SmoothIncrease(price, result.newPrice));

        price = result.newPrice;
        auctionText.SetText("$" + price.ToString("n0"));

        yield return new WaitForSeconds(0.5f);
        increasedBidText.SetText("");

        _wantValue = AuctionUtility.DecreaseWantValue(_wantValue);
    }

    private bool TryEndAuction()
    {
        if (_wantValue > 0) return false;

        StartCoroutine(FinalCountdown());
        return true;
    }

    private IEnumerator SmoothIncrease(int start, int end)
    {
        int tempPrice = start;

        while (tempPrice < end)
        {
            tempPrice += AuctionUtility.GetSmoothStep(end);
            tempPrice = Mathf.Min(tempPrice, end);

            auctionText.SetText("$" + tempPrice.ToString("n0"));
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator FinalCountdown()
    {
        auctionPromptText.SetText("GOING ONCE");
        yield return new WaitForSeconds(2);

        auctionPromptText.SetText("GOING TWICE");
        yield return new WaitForSeconds(2);

        auctionPromptText.SetText("SOLD!");
    }
}
