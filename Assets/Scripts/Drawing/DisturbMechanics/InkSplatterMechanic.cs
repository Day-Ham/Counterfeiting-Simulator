using System.Collections;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "InkSplatterMechanic", menuName = "Disturb Mechanics/Ink Splatter")]
public class InkSplatterMechanic : DisturbMechanic
{
    [SerializeField] private CanvasBrushSettings[] splatterBrushSettings;
    [SerializeField] private Color splatterColor = Color.black;
    [SerializeField] private int minDroplets = 5;
    [SerializeField] private int maxDroplets = 12;
    [SerializeField] private float splatterSpread = 150f;

    public override IEnumerator Execute(RenderTexture target, CanvasStamper stamper)
    {
        Vector2 center = new Vector2(Random.Range(0, target.width), Random.Range(0, target.height));

        var centerSettings = splatterBrushSettings[0];
        stamper.StampTexture(target, centerSettings.BrushTexture, splatterColor, centerSettings.BrushSize, center);

        yield return new WaitForSeconds(0.03f);

        int count = Random.Range(minDroplets, maxDroplets + 1);
        for (int i = 0; i < count; i++)
        {
            var settings = splatterBrushSettings[Random.Range(0, splatterBrushSettings.Length)];

            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(splatterSpread * 0.15f, splatterSpread);
            float sizeFactor = Mathf.Lerp(1f, 0.35f, radius / splatterSpread);
            float brushSize = settings.BrushSize * sizeFactor;

            Vector2 pos = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            if (pos.x < 0 || pos.x >= target.width || pos.y < 0 || pos.y >= target.height) continue;

            stamper.StampTexture(target, settings.BrushTexture, splatterColor, brushSize, pos);

            yield return new WaitForSeconds(Random.Range(0.01f, 0.05f));
        }
    }
}