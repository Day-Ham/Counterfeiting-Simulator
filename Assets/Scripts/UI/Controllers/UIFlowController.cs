using System.Collections.Generic;
using UnityEngine;


public class UIFlowController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private IntEvent startUIFlowEvent;
    [SerializeField] private IntEvent batchEvent;
    [SerializeField] private IntEvent registerElementEvent;
    [SerializeField] private VoidEvent elementCompleteEvent;
    [SerializeField] private VoidEvent onFlowComplete;
    

    private int _expected;
    private int _completed;

    private void OnEnable()
    {
        startUIFlowEvent.Register(StartBatch);
        registerElementEvent.Register(OnRegister);
        elementCompleteEvent.Register(OnComplete);
    }

    private void OnDisable()
    {
        startUIFlowEvent.Unregister(StartBatch);
        registerElementEvent.Unregister(OnRegister);
        elementCompleteEvent.Unregister(OnComplete);
    }

    private void StartBatch(int batchIndex)
    {
        _expected = 0;
        _completed = 0;

        batchEvent.Raise(batchIndex);

        if (_expected == 0)
        {
            onFlowComplete?.Raise();
        }
    }

    private void OnRegister(int value)
    {
        _expected += value;
    }

    private void OnComplete()
    {
        _completed++;

        if (_completed >= _expected && _expected > 0)
        {
            onFlowComplete?.Raise();
        }
    }
}
