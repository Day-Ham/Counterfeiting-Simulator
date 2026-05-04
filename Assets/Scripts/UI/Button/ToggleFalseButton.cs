using UnityEngine;
using UnityEngine.UI;

public class ToggleFalseButton : MonoBehaviour
{
    [SerializeField] private BoolEvent toggleEvent;
    [SerializeField] private Button button;

    private void Awake() => button.onClick.AddListener(() => toggleEvent.Raise(false));
}
