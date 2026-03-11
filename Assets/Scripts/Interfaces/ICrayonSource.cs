using System.Collections.Generic;
using UnityEngine;

public interface ICrayonSource
{
    List<Color> GetActiveColors();
    GameObjectListValue GetColorBlobs();
    void InitializeColors();
}
