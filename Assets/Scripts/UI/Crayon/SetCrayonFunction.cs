using UnityEngine;

public class SetCrayonFunction : MonoBehaviour
{
    [SerializeField] private Transform _crayonParent;
    
    public void SetupCrayons(ColorDataListValue colorDataListValue)
    {
        if (colorDataListValue == null || colorDataListValue.Value == null) return;

        var crayons = _crayonParent.GetComponentsInChildren<CrayonUIItem>();
        int count = Mathf.Min(colorDataListValue.Value.Count, crayons.Length);

        for (int i = 0; i < count; i++)
        {
            if (crayons[i] != null)
            {
                crayons[i].Setup(colorDataListValue.Value[i]);
            }
        }
    }
}
