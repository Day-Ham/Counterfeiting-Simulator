using System.Collections.Generic;
using UnityEngine;

public abstract class GameModeCrayon : ScriptableObject
{
    public abstract void InitializeColors();

    // Return active colors
    public abstract List<Color> GetActiveColors();

    // Return crayon prefabs/blobs
    public abstract GameObjectListValue GetColorBlobs();
}
