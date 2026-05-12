using UnityEngine;
using TMPro;

public class StrokeCountUI : MonoBehaviour
{
    public CanvasDrawControllerValue CanvasDrawControllerValue;
    public TMP_Text StrokeCountText;
    
    private const string STROKES_REMAINING = "Strokes remaining: ";
    
    private void Start()
    {
        UpdateStrokeText();
    }

    private void Update()
    {
        UpdateStrokeText();
    }

    private void UpdateStrokeText()
    {
        if (!CanvasDrawControllerValue || !CanvasDrawControllerValue.Value) return;

        int remainingStroke = CanvasDrawControllerValue.Value.RemainingStroke;
        StrokeCountText.text = STROKES_REMAINING + remainingStroke;
    }
}
