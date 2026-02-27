using System;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using DaeHanKim.ThisIsTotallyADollar.Utility;
using UnityEngine;
using ES3Internal;
using System.IO;

namespace DaeHanKim.ThisIsTotallyADollar.Core
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        [Header("Game Manager Value")]
        [SerializeField] private GameManagerValue _gameManagerValue;
        
        [Header("Dependencies")]
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] CanvasDrawControllerValue _canvasDrawController;
        [SerializeField] ComputeShader _similarityComputeShader;
        [SerializeField] SpriteContainerRuntimeAsset _finalSpriteContainer;
        [SerializeField] SpriteRenderer _finalSpriteRenderer;
        
        [Header("GameManager Events")]
        [SerializeField] GameManagerEvents _finishGameRequestEvent;
        [SerializeField] ComparisonResultEvent _comparisonResultEvent;

        [Header("Settings/LevelConfig")]
        [Tooltip("The texture that the player needs to draw and match exactly.")]
        [SerializeField] private LevelConfigRuntimeAsset levelConfigRuntime;
        
        [Tooltip("Optional. The texture that the player starts with.")]
        [SerializeField] Texture _optionalStartingTexture;
        [SerializeField] Vector2 _finalSpritePivotPoint = new(0.5f, 0.5f); // Between (0, 0) and (1, 1)

        TextureUtility _textureUtility;
        private CanvasDrawController _canvasDraw;
        public float allSimilarity = 1f;
        public float FirstTwoDigits;
        public float LastTwoDigits;
        public bool GameIsPaused;
        
        private void OnEnable()
        {
            _finishGameRequestEvent.OnRaised += FinishGame;
        }

        private void OnDisable()
        {
            _finishGameRequestEvent.OnRaised -= FinishGame;
        }
        
        private void Awake()
        {
            _gameManagerValue.Value = this;
            
            _textureUtility = new TextureUtility(_similarityComputeShader);
            _textureUtility.Create();
        }

        private void Start()
        {
            _canvasDraw = _canvasDrawController.Value;

            if (_canvasDraw == null)
            {
                Debug.LogError("CanvasDrawController not found.");
                enabled = false;
                return;
            }
            
            _inputHandler.Initialize(_canvasDraw, FinishGame);
            
            Texture goalTexture = levelConfigRuntime.Value.TargetTexture.Value;

            _canvasDraw.OnStart(new Vector2Int(goalTexture.width, goalTexture.height));

            _canvasDraw.SetBrushColorIndex(0);

            if (_optionalStartingTexture != null)
            {
                _canvasDraw.CopyTextureToCurrentLayer(_optionalStartingTexture);
            }
        }

        private void Update()
        {
            if (GameIsPaused) return;
            
            _inputHandler.UpdateInput();
            _canvasDraw.Tick();
        }

        private void FinishGame()
        {
            allSimilarity = 1f;
            
            CanvasState playerCanvasState = _canvasDraw.MainCanvasState;

            foreach (RenderTexture playerTex in playerCanvasState.LayersRenderTextures)
            {
                Texture goalTexture = levelConfigRuntime.Value.TargetTexture.Value;

                float? similarity = _textureUtility.GetSimilarity(goalTexture, playerTex);

                if (similarity.HasValue)
                {
                    allSimilarity *= similarity.Value;
                }
                else
                {
                    Debug.LogError("Failed to get similarity!");
                    return;
                }
            }

            SaveFinalTextureToSprite(playerCanvasState);

            Debug.Log($"Game finished with similarity of {allSimilarity:0.0000}!");
            float f = Mathf.Round(allSimilarity * 10000) / 10000.0f;
            //Debug.Log(f * 10000 + "%");
            LastTwoDigits= (f * 1000 % 10) * 10;
            FirstTwoDigits = (f * 10000 - LastTwoDigits) / 100;
            Debug.Log(FirstTwoDigits + "." + (int)LastTwoDigits + "%");
            
            _comparisonResultEvent.Raise(allSimilarity, FirstTwoDigits, LastTwoDigits);
        }

        private void SaveFinalTextureToSprite(CanvasState playerCanvasState)
        {
            RenderTexture renderTexture = playerCanvasState.LayersRenderTextures[0];

            // Convert to Texture2D
            Texture2D finalTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

            RenderTexture.active = renderTexture;
            finalTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            finalTexture.Apply();
            RenderTexture.active = null;
            
            Sprite previewSprite = Sprite.Create(
                finalTexture,
                new Rect(0, 0, finalTexture.width, finalTexture.height),
                _finalSpritePivotPoint
            );

            SetPreviewSprite(previewSprite);
            
            byte[] pngBytes = finalTexture.EncodeToPNG();
            
            string fileName = $"Drawing_{DateTime.Now:yyyyMMdd_HHmmss}.es3";
            ES3Settings settings = new ES3Settings(fileName);
            string key = "Drawing";
            ES3.Save(key, pngBytes, settings);

            Debug.Log($"Saved drawing under filename: {fileName}");
        }
        
        private void SetPreviewSprite(Sprite sprite)
        {
            if (_finalSpriteRenderer)
            {
                _finalSpriteRenderer.sprite = sprite;
            };
        }

        private void OnDestroy()
        {
            _textureUtility?.Destroy();
        }

        public void PauseGame()
        {
            GameIsPaused = true;
        }

        public void ResumeGame()
        {
            GameIsPaused = false;
        }
    }
}
