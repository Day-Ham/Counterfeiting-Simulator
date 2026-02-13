using UnityEngine;

public class SetCrayonFunction : MonoBehaviour
{
    [SerializeField] private ColorDataListValue Palette;
    [SerializeField] private Transform CrayonParent;
    
    public void SetupCrayons()
    {
        if (Palette == null || Palette.Value == null || CrayonParent == null)
        {
            return;
        }

        CrayonUIItem[] crayons = CrayonParent.GetComponentsInChildren<CrayonUIItem>();

        int count = Mathf.Min(Palette.Value.Count, crayons.Length);

        for (int i = 0; i < count; i++)
        {
            if (crayons[i] == null)
            {
                continue;
            }

            crayons[i].Setup(Palette.Value[i]);
        }
    }
}
