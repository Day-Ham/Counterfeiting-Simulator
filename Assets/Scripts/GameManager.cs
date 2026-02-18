using DaeHanKim.ThisIsTotallyADollar.Drawing;
using DaeHanKim.ThisIsTotallyADollar.Utility;
using UnityEngine;

namespace DaeHanKim.ThisIsTotallyADollar.Core
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        [Header("Game Manager Value")]
        [SerializeField] private GameManagerValue _gameManagerValue;
        
        [Header("Dependencies")]
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
        public bool GameIsPaused = false;
        
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
            
            Texture goalTexture = levelConfigRuntime.Value.GoalTexture.Value;

            _canvasDraw.OnStart(new Vector2Int(goalTexture.width, goalTexture.height));

            _canvasDraw.SetBrushColorIndex(0);

            if (_optionalStartingTexture != null)
            {
                _canvasDraw.CopyTextureToCurrentLayer(_optionalStartingTexture);
            }
        }

        private void Update()
        {
            if (GameIsPaused == false)
            {
                UpdateFromUserInput();
                _canvasDraw.Tick();
            }
        }

        private void UpdateFromUserInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _canvasDraw.SetBrushColorIndex(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _canvasDraw.SetBrushColorIndex(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _canvasDraw.SetBrushColorIndex(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _canvasDraw.SetBrushColorIndex(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _canvasDraw.SetBrushColorIndex(4);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                FinishGame();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                _canvasDraw.UndoLastDraw();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                _canvasDraw.ClearCurrentLayer();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _canvasDraw.CurrentDrawMode = CanvasDrawController.DrawMode.Draw;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                _canvasDraw.CurrentDrawMode = CanvasDrawController.DrawMode.Erase;
            }
        }

        private void FinishGame()
        {
            allSimilarity = 1f;
            
            CanvasState playerCanvasState = _canvasDraw.MainCanvasState;

            foreach (RenderTexture playerTex in playerCanvasState.LayersRenderTextures)
            {
                Texture goalTexture = levelConfigRuntime.Value.GoalTexture.Value;

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
            Sprite finalSprite = _textureUtility.CreateSpriteFromRenderTexture(playerCanvasState.LayersRenderTextures[0], _finalSpritePivotPoint);
            _finalSpriteContainer.Sprite = finalSprite;

            // Optionally set a sprite renderer to use the final sprite
            if (_finalSpriteRenderer != null)
            {
                _finalSpriteRenderer.sprite = finalSprite;
            }
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
