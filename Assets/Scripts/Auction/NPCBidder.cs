using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCBidder", menuName = "Auction/NPC Bidder")]
public class NPCBidder : ScriptableObject
{
    public string NpcName;

    [Header("Bidding Behavior")]
    public int MinBidMultiplier = 1;
    public int MaxBidMultiplier = 5;

    [Header("Economy")]
    public int MaxMoney = 1000000;

    [Header("Personality")]
    [Range(0f, 1f)] public float Aggressiveness = 0.5f;
}
