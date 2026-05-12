using System.Collections;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;

public abstract class DisturbMechanic : ScriptableObject
{
    [Min(0)] public float weight = 1f;
    public abstract IEnumerator Execute(RenderTexture target, CanvasStamper stamper);
}