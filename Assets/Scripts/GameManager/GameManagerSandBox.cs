using DaeHanKim.ThisIsTotallyADollar.Core;
using UnityEngine;

public class GameManagerSandBox : GameManagerUnit
{
    [Header("Sandbox Settings")]
    [SerializeField] private SandboxConfigRuntimeAsset sandboxRuntime;
    [SerializeField] private Vector2Int sandboxCanvasSize = new(1024, 1024);

    protected override void InitializeGameMode()
    {
        if (sandboxRuntime == null || sandboxRuntime.Value == null)
        {
            Debug.LogError("SandboxRuntimeAsset not assigned!");
            return;
        }

        _canvasDraw.RuntimeAsset = sandboxRuntime;

        _canvasDraw.OnStart(sandboxCanvasSize);

        _canvasDraw.SetBrushColorIndex(0);

        _canvasDraw.IsCanDraw = true;
    }

    protected override void FinishGame()
    {
        Debug.Log("Sandbox session finished.");
    }
}
