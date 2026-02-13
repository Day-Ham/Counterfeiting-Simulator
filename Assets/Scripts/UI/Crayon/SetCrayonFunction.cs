using UnityEngine;

public class SetCrayonFunction : MonoBehaviour
{
    [SerializeField] private Transform CrayonParent;
    [SerializeField] private PopulateCrayons PaletteSource;
    
    public void SetupCrayons()
    {
        int colorPaletteAmount = PaletteSource.ColorDataList.Value.Count;
    
        CrayonUIItem[] crayons = CrayonParent.GetComponentsInChildren<CrayonUIItem>();
        
        int count = Mathf.Min(colorPaletteAmount, crayons.Length);

        for (int i = 0; i < count; i++)
        {
            if (crayons[i] == null)
                continue;

            crayons[i].Setup(PaletteSource.ColorDataList.Value[i]);
        }
    }
}
