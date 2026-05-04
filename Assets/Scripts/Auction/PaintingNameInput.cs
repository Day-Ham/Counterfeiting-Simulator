using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] private TMP_Text errorText;

    [Header("Settings")]
    [SerializeField] private int maxPaintingNameLength = 16;
    [SerializeField] private int minPaintingNameLength = 3;

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmitClicked);
        inputField.characterLimit = maxPaintingNameLength;
        errorText.text = "";
    }

    private void OnSubmitClicked()
    {   
        string error = GetValidationError(inputField.text);
        if (error != null)
        {
            errorText.text = error;
            inputField.transform.DOShakePosition(0.3f, strength: new Vector3(8f, 0f, 0f), vibrato: 20);
            inputField.image.DOColor(Color.red, 0.15f).OnComplete(() =>
                inputField.image.DOColor(Color.white, 0.15f));
            return;
        }
        
        //Set the painting name to the input value
        paintingName.Value = inputField.text;

        errorText.text = "";

        //disable the submit button to prevent multiple submissions
        submitButton.interactable = false;

        // Register callback BEFORE triggering flow
        onFlowCompleteEvent.Register(StartBidding);

        // Trigger Batch 1 UI Flow
        startUIFlowEvent.Raise(1);
    }

    private string GetValidationError(string name)
    {
        // Check if the name is null, empty, or consists only of whitespace
        if (string.IsNullOrWhiteSpace(name)) return "Name cannot be empty.";
        // Check if the name is less than the minimum length
        if (name.Trim().Length < minPaintingNameLength) return $"Name must be at least {minPaintingNameLength} characters long.";
        // Check if the name exceeds the maximum length
        if (name.Length > maxPaintingNameLength) return $"Name cannot exceed {maxPaintingNameLength} characters.";
        // Check if the name contains any invalid characters
        if (!System.Text.RegularExpressions.Regex.IsMatch(name.Trim(), @"^[a-zA-Z0-9\s]+$")) return "Name contains invalid characters.";
        // Check if the name only contains numbers
        if(int.TryParse(name.Trim(), out _)) return "Name cannot consist only of numbers.";
        
        return null;
    }

    private void StartBidding()
    {
        onFlowCompleteEvent.Unregister(StartBidding);
        beginBidEvent.Raise();
    }
}
