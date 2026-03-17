using DaeHanKim.ThisIsTotallyADollar.Core;
using UnityEngine;

public class GameManagerSandBox : GameManagerUnit
{
    [Header("Sandbox Settings")]
    [SerializeField] private SandboxConfigRuntimeAsset sandboxRuntime;
    [SerializeField] private Vector2Int sandboxCanvasSize = new(1024, 1024);

    [Header("Dependencies")]
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private UITransitionManagerValue sandboxTransitionManager;
    [SerializeField] private VoidEvent spacePressedEvent;
    
    private void OnEnable()
    {
        spacePressedEvent.Register(FinishGame);
    }

    private void OnDisable()
    {
        spacePressedEvent.Unregister(FinishGame);
    }
    
    protected override void InitializeGameMode()
    {
        SetupCanvas();
    }
    
    protected override void FinishGame()
    {
        HandleFinishTransition();
        DisableGameplay();
    }

    private void SetupCanvas()
    {
        _canvasDraw.RuntimeAsset = sandboxRuntime;
        _canvasDraw.OnStart(sandboxCanvasSize);
        _canvasDraw.SetBrushColorIndex(0);
        _canvasDraw.IsCanDraw = true;
    }
    
    private void HandleFinishTransition()
    {
        sandboxTransitionManager.Value.MoveAllOut();
    }

    private void DisableGameplay()
    {
        inputHandler?.BlockInput();
        _canvasDraw.IsCanDraw = false;
    }
    
}
