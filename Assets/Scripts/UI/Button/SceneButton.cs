using UnityEngine;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour
{
    [Header("Events")]
    [Tooltip("Assign the event to raise when this button is clicked.")]
    public VoidEvent eventToRaise;
    
    [Header("Button")]
    public Button button;

    private void Awake()
    {
        if (eventToRaise != null)
        {
            button.onClick.AddListener(() => eventToRaise.Raise());
        }
    }
}
