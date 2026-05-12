using System.Collections;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "CoffeeStainMechanic", menuName = "Disturb Mechanics/Coffee Stain")]
public class CoffeeStainMechanic : DisturbMechanic
{
    [SerializeField] private CanvasBrushSettings ringBrushSettings;
    [SerializeField] private Color stainColor = new Color(0.55f, 0.27f, 0.07f, 0.6f);
    [SerializeField] private float minRadius = 60f;
    [SerializeField] private float maxRadius = 100f;
    [SerializeField] private int stampCount = 24;
    [SerializeField] private float radiusVariance = 5f;

    public override IEnumerator Execute(RenderTexture target, CanvasStamper stamper)
    {
        Vector2 center = new Vector2(
            Random.Range(target.width * 0.2f, target.width * 0.8f),
            Random.Range(target.height * 0.2f, target.height * 0.8f)
        );

        float radius = Random.Range(minRadius, maxRadius);

        for (int i = 0; i < stampCount; i++)
        {
            float angle = (i / (float)stampCount) * Mathf.PI * 2f;
            float r = radius + Random.Range(-radiusVariance, radiusVariance);
            Vector2 pos = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;

            if (pos.x < 0 || pos.x >= target.width || pos.y < 0 || pos.y >= target.height) continue;

            stamper.StampTexture(target, ringBrushSettings.BrushTexture, stainColor, ringBrushSettings.BrushSize, pos);

            yield return null;
        }
    }
}