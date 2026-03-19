using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIFlowBatch
{
    public List<UITransitionElementValue> UIToMoveOrHide = new();
    public List<UITransitionElementValue> UIToReturnOriginalPosition = new();
}

public class UIFlowController : MonoBehaviour
{
    [Header("UI Flow Value")]
    [SerializeField] private UIFlowControllerValue UIFlowValue;
    
    [Header("Batches of UI Flows")]
    [SerializeField] private List<UIFlowBatch> uiBatches = new();

    [Header("Flow Complete Event on last element")]
    public VoidEvent OnFlowComplete;

    private void Awake()
    {
        UIFlowValue.Value = this;
    }

    public void StartBatch(int batchIndex)
    {
        if (batchIndex < 0 || batchIndex >= uiBatches.Count) return;

        var batch = uiBatches[batchIndex];

        if (batch.UIToMoveOrHide == null || batch.UIToMoveOrHide.Count == 0)
        {
            OnAllUIHidden(batch);
            return;
        }

        int completedCount = 0;

        foreach (var uiValue in batch.UIToMoveOrHide)
        {
            if (!uiValue?.Value)
            {
                completedCount++;
                CheckAllHidden(batch, completedCount);
                continue;
            }

            uiValue.Value.OnMoveOutComplete += () =>
            {
                completedCount++;
                CheckAllHidden(batch, completedCount);
            };

            uiValue.MoveOutElement();
        }
    }

    private void CheckAllHidden(UIFlowBatch batch, int completedCount)
    {
        if (completedCount >= batch.UIToMoveOrHide.Count)
        {
            OnAllUIHidden(batch);
        }
    }

    private void OnAllUIHidden(UIFlowBatch batch)
    {
        if (batch.UIToReturnOriginalPosition != null && batch.UIToReturnOriginalPosition.Count > 0)
        {
            int moveInCompleted = 0;
            int total = batch.UIToReturnOriginalPosition.Count;

            foreach (var uiValue in batch.UIToReturnOriginalPosition)
            {
                if (!uiValue?.Value)
                {
                    moveInCompleted++;
                    if (moveInCompleted >= total)
                        OnFlowComplete?.Raise();
                    continue;
                }
                
                uiValue.Value.OnMoveInComplete += () =>
                {
                    moveInCompleted++;
                    if (moveInCompleted >= total)
                        OnFlowComplete?.Raise();
                };

                uiValue.MoveInElement();
            }
        }
        else
        {
            // No elements to move in, raise the event immediately
            OnFlowComplete?.Raise();
        }
    }
}
