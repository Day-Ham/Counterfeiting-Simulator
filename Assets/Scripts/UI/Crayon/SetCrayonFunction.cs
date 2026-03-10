using System.Collections.Generic;
using UnityEngine;

public class SetCrayonFunction : MonoBehaviour
{
    [SerializeField] private Transform _crayonParent;
    
    public void SetupCrayons(List<Color> colors)
    {
        if (colors == null) return;

        var crayons = _crayonParent.GetComponentsInChildren<CrayonUIItem>();
        int count = Mathf.Min(colors.Count, crayons.Length);

        for (int i = 0; i < count; i++)
        {
            if (crayons[i] != null)
            {
                crayons[i].Setup(colors[i], i);
            }
        }
    }
}
