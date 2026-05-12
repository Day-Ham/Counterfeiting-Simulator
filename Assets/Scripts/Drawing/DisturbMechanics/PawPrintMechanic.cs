using System.Collections;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "PawPrintMechanic", menuName = "Disturb Mechanics/Paw Prints")]
public class PawPrintMechanic : DisturbMechanic
{
    [SerializeField] private CanvasBrushSettings pawBrushSettings;
    [SerializeField] private Color pawColor = new Color(0.15f, 0.08f, 0.04f, 1f);
    [SerializeField] private int stepCount = 6;
    [SerializeField] private float stepSpacing = 80f;
    [SerializeField] private float pawLateralOffset = 28f;
    [SerializeField] private float stepDelay = 0.25f;

    public override IEnumerator Execute(RenderTexture target, CanvasStamper stamper)
    {
        Vector2 start = GetRandomEdgePosition(target);
        Vector2 targetPoint = new Vector2(
            Random.Range(target.width * 0.25f, target.width * 0.75f),
            Random.Range(target.height * 0.25f, target.height * 0.75f)
        );

        Vector2 direction = (targetPoint - start).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);

        for (int i = 0; i < stepCount; i++)
        {
            float lateralSign = i % 2 == 0 ? 1f : -1f;
            Vector2 pos = start + direction * (stepSpacing * i) + perpendicular * (pawLateralOffset * lateralSign);

            if (pos.x < 0 || pos.x >= target.width || pos.y < 0 || pos.y >= target.height) break;

            stamper.StampTexture(target, pawBrushSettings.BrushTexture, pawColor, pawBrushSettings.BrushSize, pos);

            yield return new WaitForSeconds(stepDelay);
        }
    }

    private Vector2 GetRandomEdgePosition(RenderTexture rt)
    {
        int edge = Random.Range(0, 4);
        return edge switch
        {
            0 => new Vector2(Random.Range(0f, rt.width), 0f),
            1 => new Vector2(Random.Range(0f, rt.width), rt.height - 1f),
            2 => new Vector2(0f, Random.Range(0f, rt.height)),
            _ => new Vector2(rt.width - 1f, Random.Range(0f, rt.height)),
        };
    }
}