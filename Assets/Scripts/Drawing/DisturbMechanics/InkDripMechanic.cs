using System.Collections;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "InkDripMechanic", menuName = "Disturb Mechanics/Ink Drip")]
public class InkDripMechanic : DisturbMechanic
{
    [SerializeField] private CanvasBrushSettings dripBrushSettings;
    [SerializeField] private Color inkColor = Color.black;
    [SerializeField] private float dripLength = 120f;
    [SerializeField] private float stepSize = 6f;
    [SerializeField] private float minStepDelay = 0.02f;
    [SerializeField] private float maxStepDelay = 0.06f;

    public override IEnumerator Execute(RenderTexture target, CanvasStamper stamper)
    {
        Vector2 pos = new Vector2(
            Random.Range(0, target.width),
            Random.Range(target.height * 0.3f, target.height * 0.8f)
        );

        float totalSteps = dripLength / stepSize;

        for (int i = 0; i < totalSteps; i++)
        {
            if (pos.y < 0) break;

            float t = i / totalSteps;
            float brushSize = Mathf.Lerp(dripBrushSettings.BrushSize, dripBrushSettings.BrushSize * 0.2f, t);
            brushSize *= Random.Range(0.8f, 1.2f);

            stamper.StampTexture(target, dripBrushSettings.BrushTexture, inkColor, brushSize, pos);

            pos.y -= stepSize;
            float stepDelay = Random.Range(minStepDelay, maxStepDelay);
            yield return new WaitForSeconds(stepDelay);
        }
    }
}