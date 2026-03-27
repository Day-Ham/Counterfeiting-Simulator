using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaintingNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private StringValue paintingName;
    [SerializeField] private Button submitButton;
    
    [Header("Dependencies")]
    [SerializeField] private UIFlowControllerValue uiFlowControllerValue;
    [SerializeField] private AuctionMechanicValue auctionMechanicValue;

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
        
        if (uiFlowControllerValue?.Value != null)
        {
            uiFlowControllerValue.Value.OnFlowComplete.Register(StartBidding);
        }

        if (uiFlowControllerValue == null) return;
        
        if (uiFlowControllerValue.Value != null)
        {
            uiFlowControllerValue.Value.StartBatch(1);
        }
    }

    private void SetName(string value)
    {
        paintingName.Value = string.IsNullOrWhiteSpace(value) ? defaultName : value;
    }
    
    private void StartBidding()
    {
        if (uiFlowControllerValue?.Value)
        {
            uiFlowControllerValue.Value.OnFlowComplete.Unregister(StartBidding);
        }
        
        auctionMechanicValue.Value.BeginBidding();
    }
}
