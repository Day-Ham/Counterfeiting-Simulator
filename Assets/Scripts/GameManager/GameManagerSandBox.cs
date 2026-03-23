using System.Collections.Generic;
using DaeHanKim.ThisIsTotallyADollar.Core;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using DaeHanKim.ThisIsTotallyADollar.Utility;
using UnityEngine;

public class GameManagerSandBox : GameManagerUnit
{
    [Header("Sandbox Settings")]
    [SerializeField] private SandboxConfigRuntimeAsset sandboxRuntime;
    [SerializeField] private Vector2Int sandboxCanvasSize = new(1024, 1024);

    [Header("Dependencies")]
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private AuctionMechanicValue auctionMechanicValue;
    [SerializeField] private VoidEvent spacePressedEvent;
    
    [Header("UI Elements to Disable")]
    [SerializeField] private List<GameObjectValue> UIElementsToDisable;
    
    [Header("UI Flow")]
    [SerializeField] private UIFlowControllerValue UIFlowControllerValue;
    
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
        SetupCanvas();
    }
    
    protected override void FinishGame()
    {
        foreach (var uiElementGameObjectValue in UIElementsToDisable)
        {
            uiElementGameObjectValue.Value.SetActive(false);
        }
        
        StartUIFlow();
        DisableGameplay();
    }

    private void SetupCanvas()
    {
        _canvasDraw.RuntimeAsset = sandboxRuntime;
        _canvasDraw.OnStart(sandboxCanvasSize);
        _canvasDraw.SetBrushColorIndex(0);
        _canvasDraw.IsCanDraw = true;
    }
    
    private void StartUIFlow()
    {
        UIFlowControllerValue.Value.StartBatch(0);
    }

    private void DisableGameplay()
    {
        inputHandler?.BlockInput();
        _canvasDraw.IsCanDraw = false;
    }
}
