using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaintingNameInput : MonoBehaviour
{
    [Header("Events")] 
    [SerializeField] private VoidEvent beginBidEvent;

    [Header("UI Flow Events")]
    [SerializeField] private IntEvent startUIFlowEvent;
    [SerializeField] private VoidEvent onFlowCompleteEvent;

    [Header("UI")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private StringValue paintingName;
    [SerializeField] private Button submitButton;

    [Header("No Name")]
    [SerializeField] private string defaultName = "Untitled Painting";

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmitClicked);
    }

    private void OnSubmitClicked()
    {
        SetName(inputField.text);
        inputField.text = "";

        // Register callback BEFORE triggering flow
        onFlowCompleteEvent.Register(StartBidding);

        // Trigger Batch 1 UI Flow
        startUIFlowEvent.Raise(1);
    }

    private void SetName(string value)
    {
        paintingName.Value = string.IsNullOrWhiteSpace(value)
            ? defaultName
            : value;
    }

    private void StartBidding()
    {
        onFlowCompleteEvent.Unregister(StartBidding);
        beginBidEvent.Raise();
    }
}
