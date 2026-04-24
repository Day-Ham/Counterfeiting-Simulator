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
        //Check if the painting has a name
        if (!IsNameValid(inputField.text))
        {
            return;
        }
        
        //Set the painting name to the input value
        paintingName.Value = inputField.text;

        inputField.text = "";

        //disable the submit button to prevent multiple submissions
        submitButton.interactable = false;

        // Register callback BEFORE triggering flow
        onFlowCompleteEvent.Register(StartBidding);

        // Trigger Batch 1 UI Flow
        startUIFlowEvent.Raise(1);
    }

    private bool IsNameValid(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    private void StartBidding()
    {
        onFlowCompleteEvent.Unregister(StartBidding);
        beginBidEvent.Raise();
    }
}
