using System;
//using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DaeHanKim.ThisIsTotallyADollar.Drawing
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasLayerDrawController))]
    [RequireComponent(typeof(CanvasStamper))]
    public class CanvasDrawController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public enum DrawMode
        {
            Draw,
            Erase
        }

        static readonly Vector3[] LAYER_CORNER_POSITIONS = new Vector3[4];

        [SerializeField] public CanvasDrawControllerValue _canvasDrawControllerValue;

        [Header("Local Dependencies")]
        public LevelConfigRuntimeAsset LevelConfigRuntime;
        [SerializeField] GraphicRaycaster _graphicRaycaster;
        [SerializeField] CanvasLayerDrawController _layerDrawController;
        [SerializeField] CanvasStamper _canvasStamper;
        [SerializeField] CanvasDrawView _drawView;

        [field: Header("Settings")]
        [field: SerializeField, Min(1)] public int HistorySize { get; private set; } = 10;
        [SerializeField, Min(1)] int _layersCount = 1;
        [SerializeField] CanvasBrushSettings _initialCanvasBrushSettings;
        [SerializeField] Color _clearColor = Color.clear;
        [SerializeField] Color _color0 = Color.black;

        [field: Header("Debug")]
        [field: SerializeField, Utilities.ShowIf(true, nameof(IsApplicationPlaying))] public CanvasBrushRuntimeSettings CurrentBrushSettings { get; private set; }
        [field: SerializeField, Utilities.ShowIf(false, nameof(IsApplicationPlaying))] public bool IsUpdating { get; private set; }

        [NonSerialized] public CanvasState MainCanvasState;
        [NonSerialized] public CanvasState[] CanvasStateHistory;
        [NonSerialized] public DrawMode CurrentDrawMode;
        [NonSerialized] public int CurrentCanvasStateHistoryCount;
        [NonSerialized] public int OldestCanvasStateHistoryIndex;
        [NonSerialized] public int LatestCanvasStateHistoryIndex;

        CanvasOperation _queuedCanvasOperation;
        Vector2Int _canvasDimensions;

        bool IsApplicationPlaying() => Application.IsPlaying(this);
        
        private void OnEnable()
        {
                LevelConfigRuntime.OnValueChanged += OnLevelChanged;
        }

        private void OnDisable()
        {
                LevelConfigRuntime.OnValueChanged -= OnLevelChanged;
        }

        private void OnLevelChanged(LevelConfig newLevel)
        {
            SetBrushColorIndex(0);
        }

        void Awake()
        {
            OnValidate();
            
            _canvasDrawControllerValue.Value = this;
        }

        public void OnStart(Vector2Int canvasDimensions)
        {
            _canvasDimensions = canvasDimensions;

            InitializeAllCanvasStates();
            _drawView.SetImageTextures(MainCanvasState.LayersRenderTextures);
            _layerDrawController.Initialize(MainCanvasState.LayersRenderTextures);
            _layerDrawController.SetCurrentLayerIndex(0);
            _layerDrawController.ClearLayer(_clearColor);

            CurrentBrushSettings = new CanvasBrushRuntimeSettings
            {
                BrushTexture = _initialCanvasBrushSettings.BrushTexture,
                BrushColorIndex = 0,
                BrushSize = _initialCanvasBrushSettings.BrushSize
            };

            RefreshCurrentBrushSettings();
        }

        void RefreshCurrentBrushSettings()
        {
            if (CurrentBrushSettings == null)
                return;

            _layerDrawController.SetBrushTexture(CurrentBrushSettings.BrushTexture);
            SetBrushColorIndex(CurrentBrushSettings.BrushColorIndex);
            SetBrushSize(CurrentBrushSettings.BrushSize);
        }

        void InitializeAllCanvasStates()
        {
            MainCanvasState = CreateCanvasState();
            CanvasStateHistory = new CanvasState[HistorySize];

            for (int i = 0; i < HistorySize; i++)
            {
                CanvasStateHistory[i] = CreateCanvasState();
            }

            // The first canvas state in history is already initialized to be the same as the main state
            CurrentCanvasStateHistoryCount = 1;
        }

        CanvasState CreateCanvasState()
        {
            RenderTexture[] layersRenderTextures = new RenderTexture[_layersCount];

            for (int j = 0; j < _layersCount; j++)
            {
                layersRenderTextures[j] = CreateLayerRenderTexture();
            }

            return new CanvasState(layersRenderTextures);
        }

        RenderTexture CreateLayerRenderTexture()
        {
            RenderTexture tex = new(_canvasDimensions.x, _canvasDimensions.y, 0);
            tex.filterMode = FilterMode.Point;
            tex.enableRandomWrite = true;
            tex.Create();
            return tex;
        }

        //[Button, EnableIf(nameof(IsApplicationPlaying))]
        public void StampCurrentLayer(Texture brushTexture, Color brushColor, float brushSize, Vector2 texelPosition)
        {
            if (_canvasStamper == null)
            {
                Debug.LogError("Failed to stamp current layer!");
                return;
            }

            RenderTexture currentLayer = MainCanvasState.LayersRenderTextures[0];
            _canvasStamper.StampTexture(currentLayer, brushTexture, brushColor, brushSize, texelPosition);
        }

        public void Tick()
        {
            if (_queuedCanvasOperation == null)
            {
                UpdateDrawController(Input.mousePosition);
            }
            else
            {
                _queuedCanvasOperation?.Execute();
                _queuedCanvasOperation = null;
            }
        }

        void UpdateDrawController(Vector2 cursorScreenPosition)
        {
            Vector2 texelPos = GetScreenToTexelPosition(cursorScreenPosition);
            _layerDrawController.Tick(IsUpdating, CurrentDrawMode, texelPos);
        }

        Vector2 GetScreenToTexelPosition(Vector2 cursorScreenPosition)
        {
            RectTransform rectTransform = (RectTransform) _drawView.transform;
            rectTransform.GetWorldCorners(LAYER_CORNER_POSITIONS);

            float normalizedPosX = Mathf.InverseLerp(LAYER_CORNER_POSITIONS[0].x, LAYER_CORNER_POSITIONS[2].x, cursorScreenPosition.x);
            float normalizedPosY = Mathf.InverseLerp(LAYER_CORNER_POSITIONS[0].y, LAYER_CORNER_POSITIONS[2].y, cursorScreenPosition.y);

            return new Vector2(_canvasDimensions.x * normalizedPosX, _canvasDimensions.y * normalizedPosY);
        }

        public void CopyTextureToCurrentLayer(Texture copyTexture)
        {
            _layerDrawController.CopyTextureToCurrentLayer(copyTexture);
        }

        public void SetBrushColorIndex(int index)
        {
            if (LevelConfigRuntime == null || LevelConfigRuntime.Value == null)
            {
                Debug.LogWarning("LevelConfigRuntimeAsset not assigned!");
                return;
            }

            LevelConfig level = LevelConfigRuntime.Value;
            
            if (level.ColorsToBeUsed == null || level.ColorsToBeUsed.Value == null)
            {
                Debug.LogWarning("Level has no ColorsToBeUsed assigned!");
                return;
            }
            
            var colors = level.ColorsToBeUsed.Value;
            
            if (colors.Count == 0)
            {
                Debug.LogWarning("ColorsToBeUsed list is empty!");
                return;
            }

            if (index < 0 || index >= colors.Count)
            {
                Debug.LogWarning($"Invalid color index {index}. Max allowed: {colors.Count - 1}");
                return;
            }

            CurrentBrushSettings.BrushColorIndex = index;

            Color selectedColor = colors[index];
            _layerDrawController.SetBrushColor(selectedColor);
        }

        public void SetBrushSize(float brushSize)
        {
            if (_layerDrawController == null)
            {
                Debug.LogWarning("_layerDrawController is null!");
                return;
            }
            
            if (brushSize < 0f)
                return;

            CurrentBrushSettings.BrushSize = brushSize;

            _layerDrawController.SetBrushSize(CurrentBrushSettings.BrushSize);
        }

        public void ClearCurrentLayer()
        {
            _layerDrawController.ClearLayer(_clearColor);
            _queuedCanvasOperation ??= new SnapshotCurrentCanvasOperation(this);
        }

        public void UndoLastDraw()
        {
            if (_queuedCanvasOperation != null)
                return;
            if (CurrentCanvasStateHistoryCount <= 1)
                return;

            StopDrawing();
            _queuedCanvasOperation = new UndoLastDrawCanvasOperation(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StartDrawing();
        }

        void StartDrawing()
        {
            if (IsUpdating)
                return;

            IsUpdating = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StopDrawing();
        }

        void StopDrawing()
        {
            if (!IsUpdating)
                return;

            IsUpdating = false;
            _queuedCanvasOperation ??= new SnapshotCurrentCanvasOperation(this);
        }

        void OnDestroy()
        {
            foreach (CanvasState canvasState in CanvasStateHistory)
            {
                canvasState.Destroy();
            }

            MainCanvasState.Destroy();
        }

        void OnValidate()
        {
            RetrieveDependencies();
            RefreshCurrentBrushSettings();
        }

        void RetrieveDependencies()
        {
            if (_layerDrawController == null)
            {
                _layerDrawController = GetComponent<CanvasLayerDrawController>();
            }

            if (_canvasStamper == null)
            {
                _canvasStamper = GetComponent<CanvasStamper>();
            }
        }

        public void SetDrawMode(DrawMode desiredMode)
        {
            CurrentDrawMode = desiredMode;
        }
    }
}
