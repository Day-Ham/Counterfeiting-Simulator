using UnityEngine;
using UnityEngine.UI;

public class StartCompare : MonoBehaviour
{
    [SerializeField] private Button _compareButton;
    [SerializeField] private VoidEvent _startCompareEvent;

    private void Awake()
    {
        _compareButton.onClick.AddListener(RaiseCompareEvent);
    }

    private void RaiseCompareEvent()
    {
        _startCompareEvent.Raise();
    }
}
