using System.Collections;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;

public class DisturbController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEvent disturbMechanicEvent;
    [Header("References")]
    [SerializeField] private CanvasDrawController canvasDrawController;
    [SerializeField] private CanvasStamper canvasStamper;
    [Header("Mechanics")]
    [SerializeField] private DisturbMechanic[] mechanics;

    private void OnEnable() => disturbMechanicEvent.Register(TriggerDisturbMechanic);
    private void OnDisable() => disturbMechanicEvent.Unregister(TriggerDisturbMechanic);

    private void TriggerDisturbMechanic()
    {
        var rt = canvasDrawController.MainCanvasState?.LayersRenderTextures[0];
        if (rt == null) return;

        StartCoroutine(PickWeightedMechanic().Execute(rt, canvasStamper));
    }

    private DisturbMechanic PickWeightedMechanic()
    {
        float total = 0f;
        foreach (var mechanic in mechanics)
            total += mechanic.weight;

        float roll = Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var mechanic in mechanics)
        {
            cumulative += mechanic.weight;
            if (roll < cumulative)
                return mechanic;
        }

        return mechanics[mechanics.Length - 1];
    }
}