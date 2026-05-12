using System.Collections;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "SmearMechanic", menuName = "Disturb Mechanics/Smear")]
public class SmearMechanic : DisturbMechanic
{
    [SerializeField] private CanvasBrushSettings smearBrushSettings;
    [SerializeField] private Color smearColor = new Color(0.1f, 0.1f, 0.1f, 0.4f);
    [SerializeField] private float smearLength = 150f;
    [SerializeField] private float stepSize = 8f;
    [SerializeField] private float lateralVariance = 6f;
    [SerializeField] private float stepDelay = 0.02f;

    public override IEnumerator Execute(RenderTexture target, CanvasStamper stamper)
    {
        Vector2 start = new Vector2(
            Random.Range(0, target.width),
            Random.Range(0, target.height)
        );

        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);

        int totalSteps = Mathf.RoundToInt(smearLength / stepSize);

        for (int i = 0; i < totalSteps; i++)
        {
            float t = i / (float)totalSteps;
            Vector2 pos = start + direction * (stepSize * i) + perpendicular * Random.Range(-lateralVariance, lateralVariance);

            if (pos.x < 0 || pos.x >= target.width || pos.y < 0 || pos.y >= target.height) break;

            float alpha = Mathf.Lerp(smearColor.a, 0f, t);
            Color c = new Color(smearColor.r, smearColor.g, smearColor.b, alpha);

            stamper.StampTexture(target, smearBrushSettings.BrushTexture, c, smearBrushSettings.BrushSize, pos);

            yield return new WaitForSeconds(stepDelay);
        }
    }
}