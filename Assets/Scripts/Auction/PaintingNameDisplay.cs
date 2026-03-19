using TMPro;
using UnityEngine;

public class PaintingNameDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private StringValue paintingName;

    private void Start()
    {
        paintingName.Value = string.Empty;
    }

    private void OnEnable()
    {
        paintingName.OnValueChanged += UpdateText;
        UpdateText(paintingName.Value);
    }

    private void OnDisable()
    {
        paintingName.OnValueChanged -= UpdateText;
    }

    private void UpdateText(string value)
    {
        displayText.text = string.IsNullOrEmpty(value) ? "" : value;
    }
}
