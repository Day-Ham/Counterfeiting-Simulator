using DaeHanKim.ThisIsTotallyADollar.Core;
using DaeHanKim.ThisIsTotallyADollar.Utility;
using UnityEngine;

public class GameManagerSandBox : GameManagerUnit
{
    [Header("Events")]
    [SerializeField] private VoidEvent spacePressedEvent;
    [SerializeField] private IntEvent startUIFlowEvent;
    [SerializeField] private VoidEvent drawingBoardControllerEvent;

    
    [Header("Sandbox Settings")]
    [SerializeField] private SandboxConfigRuntimeAsset sandboxRuntime;
    [SerializeField] private Vector2Int sandboxCanvasSize = new(1024, 1024);
    
    private TextureUtility _textureUtility;
    
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
        GameState.GameStart();
        
        SetupCanvas();
    }
    
    protected override void FinishGame()
    {
        GameState.FinishGame();

        // Notify DrawingBoardController to reset position and sorting order for next game
        drawingBoardControllerEvent?.Raise();
        
        StartUIFlow();
    }

    private void SetupCanvas()
    {
        CanvasDraw.RuntimeAsset = sandboxRuntime;
        CanvasDraw.OnStart(sandboxCanvasSize);
        CanvasDraw.SetBrushColorIndex(0);
        CanvasDraw.IsCanDraw = true;
    }
    
    private void StartUIFlow()
    {
        startUIFlowEvent.Raise(0);
    }
}
