using System;
using UnityEngine;

namespace DaeHanKim.ThisIsTotallyADollar.Drawing
{
    [Serializable]
    public class CanvasBrushRuntimeSettings
    {
        [SerializeField] public Texture BrushTexture;
        [SerializeField] public int BrushColorIndex;
        [SerializeField] public float BrushSize;
    }
}
