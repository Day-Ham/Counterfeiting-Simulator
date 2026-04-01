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
    
    private int _totalOperations;
    private int _completedOperations;

    private void Awake()
    {
        UIFlowValue.Value = this;
    }

    public void StartBatch(int batchIndex)
    {
        if (batchIndex < 0 || batchIndex >= uiBatches.Count) return;

        var batch = uiBatches[batchIndex];

        _totalOperations = 0;
        _completedOperations = 0;
        
        // MOVE OUT / HIDE (PARALLEL)
        if (batch.UIToMoveOrHide != null)
        {
            foreach (var uiValue in batch.UIToMoveOrHide)
            {
                _totalOperations++;

                if (!uiValue?.Value)
                {
                    IncrementComplete();
                    continue;
                }

                uiValue.Value.OnMoveOutComplete += OnOperationComplete;
                uiValue.MoveOutElement();
            }
        }
        
        // MOVE IN (PARALLEL)
        if (batch.UIToReturnOriginalPosition != null)
        {
            foreach (var uiValue in batch.UIToReturnOriginalPosition)
            {
                _totalOperations++;

                if (!uiValue?.Value)
                {
                    IncrementComplete();
                    continue;
                }

                uiValue.Value.OnMoveInComplete += OnOperationComplete;
                uiValue.MoveInElement();
            }
        }

        // Edge case: nothing to process
        if (_totalOperations == 0)
        {
            OnFlowComplete?.Raise();
        }
    }

    private void OnOperationComplete()
    {
        _completedOperations++;

        if (_completedOperations >= _totalOperations)
        {
            OnFlowComplete?.Raise();
        }
    }

    private void IncrementComplete()
    {
        _completedOperations++;

        if (_completedOperations >= _totalOperations)
        {
            OnFlowComplete?.Raise();
        }
    }
}
