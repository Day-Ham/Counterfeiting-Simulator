using System.Collections.Generic;
using UnityEngine;

public class RuntimeWhiteColorData
{
    public List<Color> WhiteColors { get; private set; }

    public RuntimeWhiteColorData(List<Color> source)
    {
        WhiteColors = new List<Color>(source);
    }
}
