using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCBidder", menuName = "Auction/NPC Bidder")]
public class NPCBidder : ScriptableObject
{
    public string npcName;

    [Header("Bidding Behavior")]
    public int minBidMultiplier = 1;
    public int maxBidMultiplier = 5;

    [Header("Economy")]
    public int maxMoney = 1000000;

    [Header("Personality")]
    [Range(0f, 1f)] public float aggressiveness = 0.5f;
}
