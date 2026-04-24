using UnityEngine;

[CreateAssetMenu(fileName = "NewHintData", menuName = "Hint/Hint Data")]
public class HintData : ScriptableObject
{
    [Header("Hint Settings")]
    //The message to display in the hint
    public string message;
    //Delay before showing the hint
    public float delay = 10f;
    //Duration to display the hint before it disappears
    public float displayDuration = 5f;
    //Position of the hint on the screen
    public Vector2 screenPosition;
    //Bool if use arrow pointing to the relevant UI element
    public bool useArrow;
    //Position of the arrow pointing to the relevant UI element
    public Vector2 arrowPosition;
    //Rotation of the arrow pointing to the relevant UI element
    public float arrowRotation;
}
