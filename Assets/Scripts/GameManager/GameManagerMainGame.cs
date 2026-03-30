using DaeHanKim.ThisIsTotallyADollar.Core;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using DaeHanKim.ThisIsTotallyADollar.Utility;
using UnityEngine;

public class GameManagerMainGame : GameManagerUnit
{
    [Header("Similarity System")]
    [SerializeField] private ComputeShader _similarityComputeShader;
    
    [Header("Events")]
    [SerializeField] private VoidEvent _finishGameRequestEvent;
    [SerializeField] private ComparisonResultEvent _comparisonResultEvent;
    
    [Header("MainGame Settings")]
    [SerializeField] private LevelConfigRuntimeAsset levelConfigRuntime; 
    [SerializeField] private Texture _optionalStartingTexture; 
    [SerializeField] private Vector2 _finalSpritePivotPoint = new(0.5f, 0.5f);

    [Header("Final Sprite")]
    [SerializeField] private SpriteContainerRuntimeAsset _finalSpriteContainer;
    [SerializeField] private SpriteRenderer _finalSpriteRenderer;

    private TextureUtility _textureUtility;

    private float _allSimilarity = 1f;
    private float _firstTwoDigits;
    private float _lastTwoDigits;

    private void Awake()
    {
        _textureUtility = new TextureUtility(_similarityComputeShader);
        _textureUtility.Create();
    }

    private void OnEnable()
    {
        _finishGameRequestEvent?.Register(FinishGame);
    }

    private void OnDisable()
    {
        _finishGameRequestEvent?.Unregister(FinishGame);
    }

    protected override void InitializeGameMode()
    {
        if (levelConfigRuntime == null || levelConfigRuntime.Value.TargetTexture == null)
        {
            Debug.LogError("LevelConfigRuntime or TargetTexture missing!");
            return;
        }

        Texture goalTexture = levelConfigRuntime.Value.TargetTexture.Value;

        _canvasDraw.RuntimeAsset = levelConfigRuntime;

        _canvasDraw.OnStart(new Vector2Int(goalTexture.width, goalTexture.height));

        _canvasDraw.SetBrushColorIndex(0);

        if (_optionalStartingTexture != null)
        {
            _canvasDraw.CopyTextureToCurrentLayer(_optionalStartingTexture);
        }
        
        GameState.GameStart();
    }

    protected override void FinishGame()
    {
        _allSimilarity = 1f;
        
        GameState.FinishGame();

        CanvasState playerCanvasState = _canvasDraw.MainCanvasState;

        foreach (RenderTexture playerTex in playerCanvasState.LayersRenderTextures)
        {
            Texture goalTexture = levelConfigRuntime.Value.TargetTexture.Value;

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

        SaveFinalTextureToSprite(playerCanvasState);

        float f = Mathf.Round(_allSimilarity * 10000) / 10000.0f;

        _lastTwoDigits = (f * 1000 % 10) * 10;
        _firstTwoDigits = (f * 10000 - _lastTwoDigits) / 100;

        Debug.Log($"Game finished with similarity of {_firstTwoDigits}.{(int)_lastTwoDigits}%");

        _comparisonResultEvent.Raise(_allSimilarity, _firstTwoDigits, _lastTwoDigits);
    }

    private void SaveFinalTextureToSprite(CanvasState playerCanvasState)
    {
        Sprite finalSprite = _textureUtility.CreateSpriteFromRenderTexture(playerCanvasState.LayersRenderTextures[0], _finalSpritePivotPoint);

        _finalSpriteContainer.Sprite = finalSprite;

        if (_finalSpriteRenderer)
        {
            _finalSpriteRenderer.sprite = finalSprite;
        }
    }

    private void OnDestroy()
    {
        _textureUtility?.Destroy();
    }
}
