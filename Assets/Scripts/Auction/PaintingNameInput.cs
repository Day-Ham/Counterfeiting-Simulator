using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaintingNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private StringValue paintingName;
    [SerializeField] private Button submitButton;

    [SerializeField] private string defaultName = "Untitled Painting";

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmitClicked);
    }
    
    private void OnSubmitClicked()
    {
        SetName(inputField.text);
        inputField.text = "";
    }

    private void SetName(string value)
    {
        paintingName.Value = string.IsNullOrWhiteSpace(value) ? defaultName : value;
    }
}
