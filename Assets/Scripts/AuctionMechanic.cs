using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AuctionMechanic : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI auctiontext;
    [SerializeField] TextMeshProUGUI increasedbid;
    //Temporary
    //Auction Indicator Handler Must be made
    [SerializeField] TextMeshProUGUI AuctionPrompt;
    [SerializeField] int price = 0;
    int wantvalue = 100;
    int duration = 30;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //auctiontext.SetText("$"+price.ToString("n0"));
    }

    public void BeginBidding()
    {
        int StartingPrice = Random.Range(100000, 1000000);
        int Digits = Mathf.FloorToInt(Mathf.Log10(StartingPrice));
        
        int BiddingRange = Mathf.FloorToInt(Mathf.Pow(10, Digits - 1));
        
        int RandomValue= BiddingRange * Random.Range(1, 75);
        price = RandomValue;
        wantvalue = 100;
        StartCoroutine(Bidding());
    }

    private IEnumerator Bidding()
    {
        
        int countdown = 3;
        bool inAuction = true;
        while (inAuction)
        {
        AuctionPrompt.SetText(" ");
        yield return new WaitForSeconds(1);
        int Digits= Mathf.FloorToInt(Mathf.Log10(price));
            
        int BiddingRange = Mathf.FloorToInt(Mathf.Pow(10, Digits - Random.Range(1, 3)));
        int AddedBid =BiddingRange * Random.Range(1, 5);
        int AddedBidDigits = Mathf.FloorToInt(Mathf.Log10(AddedBid));
        Debug.Log(AddedBidDigits);
        //ADD INDICATOR HOOK HERE
        increasedbid.SetText("$" + AddedBid.ToString("n0"));
        int newprice =price + AddedBid;
  

            while(newprice > price)
            {
                yield return new WaitForSeconds(0.03f);
                if (AddedBidDigits <= 4)
                {
                    price += Random.Range(100, 1000);
                }
                if (AddedBidDigits == 5)
                {
                    price += Random.Range(10000, 50000);
                }
                else if (AddedBidDigits == 6)
                {
                    price += Random.Range(100000, 500000);
                }
                else if(AddedBidDigits == 7)
                {
                    price += Random.Range(1000000, 5000000);
                }
                auctiontext.SetText("$" + price.ToString("n0"));


            }
            price = newprice;
            auctiontext.SetText("$" + price.ToString("n0"));
            yield return new WaitForSeconds(.5f);
            increasedbid.SetText("");

            wantvalue = wantvalue - Random.Range(1, 15);
            if(Random.Range(1, 25) == 1)
            {


                wantvalue = 100;
            }
            if (wantvalue < 0)
            {
                bool sold = false;
                //BEGIN COUNTDOWN
                while (!sold )
                {

                    int final = Random.Range(1,9);
                    yield return new WaitForSeconds(1);

                    if (countdown == 1)
                    {
                        AuctionPrompt.SetText("GOING TWICE");
                        yield return new WaitForSeconds(3);

                    }else if(countdown == 2)
                    {
                        AuctionPrompt.SetText("GOING ONCE");
                    }
                    if (final == 1)
                    {

                        wantvalue = 100;
                        break;
                        
                        
                    }
                    else
                    {

                        countdown--;
                        

                    }

                    if (countdown <= 0)
                    {
                        sold = true;
                        inAuction = false;
                        AuctionPrompt.SetText("SOLD");
                    }
                }
            }

        }

            
    }
}
