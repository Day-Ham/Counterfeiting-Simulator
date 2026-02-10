using UnityEngine;

public class SetCrayonFunction : MonoBehaviour
{
    [SerializeField] private ColorDataListValue Palette;
    [SerializeField] private Transform CrayonParent;
    
    private CrayonUIItem[] Crayons;
    
    private void OnValidate()
    {
        if (Palette == null || Palette.Value == null || CrayonParent == null)
        {
            return;
        }

        Crayons = CrayonParent.GetComponentsInChildren<CrayonUIItem>();

        int count = Mathf.Min(Palette.Value.Count, Crayons.Length);

        for (int i = 0; i < count; i++)
        {
            if (Crayons[i] == null)
            {
                continue;
            }

            Crayons[i].Setup(Palette.Value[i]);
        }
    }
}
