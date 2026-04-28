using UnityEngine;
using UnityEngine.UI;

public class GoBackPanelButton : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEvent ShowPreviousPanelEvent;

    [Header("Button")]
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        ShowPreviousPanelEvent?.Raise();
    }
}
