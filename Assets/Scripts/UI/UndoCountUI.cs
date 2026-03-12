using TMPro;
using UnityEngine;

public class UndoCountUI : MonoBehaviour
{
    public CanvasDrawControllerValue CanvasDrawControllerValue;
    public TMP_Text UndoCountText;
    
    private const string UNDO_REMAINING = "Undo remaining: ";
    
    private void Start()
    {
        UpdateUndoText();
    }

    private void Update()
    {
        UpdateUndoText();
    }

    private void UpdateUndoText()
    {
        if (!CanvasDrawControllerValue || !CanvasDrawControllerValue.Value) return;

        int remainingUndo = CanvasDrawControllerValue.Value.RemainingUndo;
        UndoCountText.text = UNDO_REMAINING + remainingUndo;
    }
}
