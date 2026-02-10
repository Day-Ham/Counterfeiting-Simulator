using UnityEngine;
using UnityEngine.UI;

public class EresaerUIItem : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    
    private void Awake()
    {
        Button.onClick.AddListener(Select);
    }
    
    private void Select()
    {
        Debug.Log("Eraser clicked", this);
        SelectColorEvent.RaiseErase();
    }
}
