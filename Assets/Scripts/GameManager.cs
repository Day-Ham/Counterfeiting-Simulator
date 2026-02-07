using DaeHanKim.ThisIsTotallyADollar.Drawing;
using DaeHanKim.ThisIsTotallyADollar.Utility;
using UnityEngine;

namespace DaeHanKim.ThisIsTotallyADollar.Core
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] CanvasDrawController _canvasDrawController;
        [SerializeField] ComputeShader _similarityComputeShader;
        [SerializeField] SpriteContainerRuntimeAsset _finalSpriteContainer;
        [SerializeField] SpriteRenderer _finalSpriteRenderer;

        [Header("Settings")]
        [Tooltip("The texture that the player needs to draw and match exactly.")]
        [SerializeField] Texture _goalTexture;
        [Tooltip("Optional. The texture that the player starts with.")]
        [SerializeField] Texture _optionalStartingTexture;
        [SerializeField] Vector2 _finalSpritePivotPoint = new(0.5f, 0.5f); // Between (0, 0) and (1, 1)

        TextureUtility _textureUtility;
        public float allSimilarity = 1f;
        public float FirstTwoDigits;
        public float LastTwoDigits;
        public bool GameIsPaused=false;
        void Awake()
        {
            _textureUtility = new TextureUtility(_similarityComputeShader);
            _textureUtility.Create();
        }

        void Start()
        {
            if (_goalTexture == null)
            {
                Debug.LogError("Failed to start game! Goal texture is null!");
                return;
            }

            _canvasDrawController.OnStart(new Vector2Int(_goalTexture.width, _goalTexture.height));
            _canvasDrawController.SetBrushColorIndex(0);

            if (_optionalStartingTexture != null)
            {
                _canvasDrawController.CopyTextureToCurrentLayer(_optionalStartingTexture);
            }
        }

        void Update()
        {
            if (GameIsPaused == false)
            {
                UpdateFromUserInput();
                _canvasDrawController.Tick();
            }
        }

        void UpdateFromUserInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _canvasDrawController.SetBrushColorIndex(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _canvasDrawController.SetBrushColorIndex(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _canvasDrawController.SetBrushColorIndex(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _canvasDrawController.SetBrushColorIndex(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _canvasDrawController.SetBrushColorIndex(4);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                FinishGame();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                _canvasDrawController.UndoLastDraw();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                _canvasDrawController.ClearCurrentLayer();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _canvasDrawController.CurrentDrawMode = CanvasDrawController.DrawMode.Draw;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                _canvasDrawController.CurrentDrawMode = CanvasDrawController.DrawMode.Erase;
            }
        }

        public void FinishGame()
        {
            CanvasState playerCanvasState = _canvasDrawController.MainCanvasState;

            foreach (RenderTexture playerTex in playerCanvasState.LayersRenderTextures)
            {
                float? similarity = _textureUtility.GetSimilarity(_goalTexture, playerTex);

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
        }

        void SaveFinalTextureToSprite(CanvasState playerCanvasState)
        {
            Sprite finalSprite = _textureUtility.CreateSpriteFromRenderTexture(playerCanvasState.LayersRenderTextures[0], _finalSpritePivotPoint);
            _finalSpriteContainer.Sprite = finalSprite;

            // Optionally set a sprite renderer to use the final sprite
            if (_finalSpriteRenderer != null)
            {
                _finalSpriteRenderer.sprite = finalSprite;
            }
        }

        void OnDestroy()
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
