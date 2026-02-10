using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CardBoardManager : MonoBehaviour
{
    [Header("CardBoard Manager")]
    [SerializeField] private List<CardBoardUI> cardboards = new List<CardBoardUI>();

    [Header("Selection Settings")]
    [SerializeField] private float selectionInterval = 3f;

    private CardBoardUI currentSelectedCardboard;
    private float timeSinceSelection = 0f;

    [SerializeField] private float moveDuration = 3f;
    //private bool isSelecting = false;  
    void Start()
    {
        CardboardValidator();
    }

    void Update()
    {
        NewCardBoardInterval();
    }

    void CardboardValidator()
    {
        if (cardboards.Count >  0)
        {
            SelectRandomCardboard();
        }else
        {
            Debug.Log("No cardboard selected");
        }
    }

    void NewCardBoardInterval()
    {
        timeSinceSelection += Time.deltaTime;

        if (timeSinceSelection >= selectionInterval)
        {
            SelectRandomCardboard();
            timeSinceSelection = 0f;
        }
    }

    void SelectRandomCardboard()
    {
        if (cardboards.Count == 0)
        {
            Debug.LogError("List is empty");
            return;
        }

        if (cardboards.Count == 1)
        {
            if (currentSelectedCardboard != null)
            {
                currentSelectedCardboard.SetActive(false);
            }
            currentSelectedCardboard = cardboards[0];
            currentSelectedCardboard.SetActive(true);
            return;
        }

        int randomIndex;
        CardBoardUI newSelection;
        do
        {
            randomIndex = Random.Range(0, cardboards.Count);
            newSelection = cardboards[randomIndex];
        } while (newSelection == currentSelectedCardboard);

        // Deactivate current, wait for animation, then activate new
        if (currentSelectedCardboard != null)
        {
            currentSelectedCardboard.SetActive(false);

            // Wait for up-down animation to complete before moving to next
            DOVirtual.DelayedCall(moveDuration, () => {
                newSelection.SetActive(true);
                currentSelectedCardboard = newSelection;
                Debug.Log($"Selected Cardboard {randomIndex + 1}");
            });
        }
        else
        {
            newSelection.SetActive(true);
            currentSelectedCardboard = newSelection;
        }
    }

    public void SelectCardboard(CardBoardUI cardboard)
    {
        if (currentSelectedCardboard == cardboard)
        {
            return;
        }

        foreach (CardBoardUI cb in cardboards)
        {
            cb.SetActive(false);
        }

        currentSelectedCardboard = cardboard;
        currentSelectedCardboard.SetActive(true);

        timeSinceSelection = 0f;

        Debug.Log($"Manualy Selected {cardboard.name}");
    }

}
