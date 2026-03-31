using DaeHanKim.ThisIsTotallyADollar.Core;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using DaeHanKim.ThisIsTotallyADollar.Utility;
using UnityEngine;

public class GameManagerMainGame : GameManagerUnit
{
    [Header("Events")]
    [SerializeField] private VoidEvent finishGameRequestEvent;
    [SerializeField] private ComparisonResultEvent comparisonResultEvent;
    
    [Header("Similarity System")]
    [SerializeField] private ComputeShader similarityComputeShader;
    
    [Header("MainGame Settings")]
    [SerializeField] private MainGameConfigRuntimeAsset mainGameConfigRuntime; 

    private TextureUtility _textureUtility;

    private float _allSimilarity = 1f;
    private float _firstTwoDigits;
    private float _lastTwoDigits;

    private void Awake()
    {
        _textureUtility = new TextureUtility(similarityComputeShader);
        _textureUtility.Create();
    }

    private void OnEnable()
    {
        finishGameRequestEvent?.Register(FinishGame);
    }

    private void OnDisable()
    {
        finishGameRequestEvent?.Unregister(FinishGame);
    }

    protected override void InitializeGameMode()
    {
        if (mainGameConfigRuntime == null || mainGameConfigRuntime.Value.TargetTexture == null)
        {
            Debug.LogError("LevelConfigRuntime or TargetTexture missing!");
            return;
        }

        Texture goalTexture = mainGameConfigRuntime.Value.TargetTexture.Value;

        CanvasDraw.RuntimeAsset = mainGameConfigRuntime;

        CanvasDraw.OnStart(new Vector2Int(goalTexture.width, goalTexture.height));

        CanvasDraw.SetBrushColorIndex(0);
        
        GameState.GameStart();
    }

    protected override void FinishGame()
    {
        _allSimilarity = 1f;
        
        GameState.FinishGame();

        CanvasState playerCanvasState = CanvasDraw.MainCanvasState;

        foreach (RenderTexture playerTex in playerCanvasState.LayersRenderTextures)
        {
            Texture goalTexture = mainGameConfigRuntime.Value.TargetTexture.Value;

            float? similarity = _textureUtility.GetSimilarity(goalTexture, playerTex);

            if (similarity.HasValue)
            {
                _allSimilarity *= similarity.Value;
            }
            else
            {
                Debug.LogError("Failed to get similarity!");
                return;
            }
        }

        float f = Mathf.Round(_allSimilarity * 10000) / 10000.0f;

        _lastTwoDigits = (f * 1000 % 10) * 10;
        _firstTwoDigits = (f * 10000 - _lastTwoDigits) / 100;

        Debug.Log($"Game finished with similarity of {_firstTwoDigits}.{(int)_lastTwoDigits}%");

        var result = new ComparisonResultStruct(_allSimilarity, _firstTwoDigits, _lastTwoDigits);
        comparisonResultEvent.Raise(result);
    }

    private void OnDestroy()
    {
        _textureUtility?.Destroy();
    }
}
