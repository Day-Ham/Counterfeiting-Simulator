using UnityEngine;

namespace DaeHanKim.ThisIsTotallyADollar.Drawing
{
    [CreateAssetMenu(menuName = "Settings/Canvas/Brush")]
    public class CanvasBrushSettings : ScriptableObject
    {
        [field: SerializeField] public Texture BrushTexture { get; private set; }
        [field: SerializeField] public float BrushSize { get; private set; } = 8f;
    }
}