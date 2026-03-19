using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaintingNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private StringValue paintingName;
    [SerializeField] private Button submitButton;
    [SerializeField] private UIFlowControllerValue UIFlowControllerValue;
    [SerializeField] private AuctionMechanicValue auctionMechanicValue;

    [SerializeField] private string defaultName = "Untitled Painting";

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmitClicked);
    }
    
    private void OnSubmitClicked()
    {
        SetName(inputField.text);
        inputField.text = "";
        
        if (UIFlowControllerValue?.Value != null)
        {
            UIFlowControllerValue.Value.OnFlowComplete.Register(StartBidding);
        }
        
        UIFlowControllerValue.Value.StartBatch(1);
    }

    private void SetName(string value)
    {
        paintingName.Value = string.IsNullOrWhiteSpace(value) ? defaultName : value;
    }
    
    private void StartBidding()
    {
        if (UIFlowControllerValue?.Value)
        {
            UIFlowControllerValue.Value.OnFlowComplete.Unregister(StartBidding);
        }
        
        auctionMechanicValue.Value.BeginBidding();
    }
}
