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

        public CanvasDrawControllerValue _canvasDrawControllerValue;
        public DrawingBoardZoom _drawingBoardZoom;
        public DrawingBoardController _drawingBoardController;
        [SerializeField] private SelectedColorEvent selectedColorEvent;

        [Header("Local Dependencies")]
        public ConfigRuntime RuntimeAsset;
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
        [NonSerialized] public bool IsCanDraw = true;

        CanvasOperation _queuedCanvasOperation;
        Vector2Int _canvasDimensions;
        int _remainingUndo;

        bool IsApplicationPlaying() => Application.IsPlaying(this);
        public int RemainingUndo => _remainingUndo;
        
        public int CurrentBrushColorIndex { get; private set; } = 0;
        
        private void OnEnable()
        {
            if (RuntimeAsset != null)
            {
                RuntimeAsset.OnValueChanged += OnRuntimeChanged;

                if (RuntimeAsset.HasValue)
                {
                    OnRuntimeChanged();
                }
            }
            
            selectedColorEvent.OnColorPicked += OnSelectedColor;
        }

        private void OnDisable()
        {
            if (RuntimeAsset != null)
            {
                RuntimeAsset.OnValueChanged -= OnRuntimeChanged;
            }
            selectedColorEvent.OnColorPicked -= OnSelectedColor;
        }

        private void OnRuntimeChanged()
        {
            SetBrushColorIndex(0);
        }
        
        private void OnSelectedColor(int index, Color newColor)
        {
            if (CurrentBrushSettings == null) return;
            
            if (CurrentBrushSettings.BrushColorIndex == index)
            {
                _layerDrawController.SetBrushColor(newColor);
            }
        }

        private void Awake()
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
            InitializeUndoLimit();
        }

        private void RefreshCurrentBrushSettings()
        {
            if (CurrentBrushSettings == null) return;

            _layerDrawController.SetBrushTexture(CurrentBrushSettings.BrushTexture);
            SetBrushColorIndex(CurrentBrushSettings.BrushColorIndex);
            SetBrushSize(CurrentBrushSettings.BrushSize);
        }

        private void InitializeAllCanvasStates()
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

        private CanvasState CreateCanvasState()
        {
            RenderTexture[] layersRenderTextures = new RenderTexture[_layersCount];

            for (int j = 0; j < _layersCount; j++)
            {
                layersRenderTextures[j] = CreateLayerRenderTexture();
            }

            return new CanvasState(layersRenderTextures);
        }

        private RenderTexture CreateLayerRenderTexture()
        {
            RenderTexture tex = new(_canvasDimensions.x, _canvasDimensions.y, 0);
            tex.filterMode = FilterMode.Point;
            tex.enableRandomWrite = true;
            tex.Create();
            
            // Clear the texture immediately
            RenderTexture active = RenderTexture.active;
            RenderTexture.active = tex;
            GL.Clear(true, true, _clearColor);
            RenderTexture.active = active;
            
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
            if(!CanDraw()) return;
            
            if (InputUtility.IsCtrlHeld)
            {
                if (IsUpdating)
                {
                    StopDrawing();
                };
                return;
            }
            
            if (_queuedCanvasOperation == null)
            {
                ApplyZoomCorrectedBrushSize();
                UpdateDrawController(Input.mousePosition);
            }
            else
            {
                _queuedCanvasOperation?.Execute();
                _queuedCanvasOperation = null;
            }
        }
        
        private void ApplyZoomCorrectedBrushSize()
        {
            if (CurrentBrushSettings == null) return;

            float zoomRatio = _drawingBoardZoom ? _drawingBoardZoom.ZoomRatio : 1f;
            _layerDrawController.SetBrushSize(CurrentBrushSettings.BrushSize / zoomRatio);
        }

        private void UpdateDrawController(Vector2 cursorScreenPosition)
        {
            Vector2 texelPos = GetScreenToTexelPosition(cursorScreenPosition);
            _layerDrawController.Tick(IsUpdating, CurrentDrawMode, texelPos);
        }

        private Vector2 GetScreenToTexelPosition(Vector2 cursorScreenPosition)
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
            if (RuntimeAsset == null)
            {
                Debug.LogWarning("RuntimeAsset not assigned!");
                return;
            }
            
            var colors = RuntimeAsset.GetActiveColors();

            if (colors == null || colors.Count == 0)
            {
                Debug.LogWarning("Active color list is empty!");
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
            
            if (brushSize < 0f) return;

            CurrentBrushSettings.BrushSize = brushSize;

            float zoomRatio = _drawingBoardZoom != null ? _drawingBoardZoom.ZoomRatio : 1f;
            _layerDrawController.SetBrushSize(CurrentBrushSettings.BrushSize / zoomRatio);
        }

        public void ClearCurrentLayer()
        {
            _layerDrawController.ClearLayer(_clearColor);
            _queuedCanvasOperation ??= new SnapshotCurrentCanvasOperation(this);
        }

        public void UndoLastDraw()
        {
            if (_queuedCanvasOperation != null) return;
            if (CurrentCanvasStateHistoryCount <= 1) return;
            
            if (_remainingUndo == 0 && RuntimeAsset.UndoLimit > 0)
            {
                Debug.Log("Undo limit reached.");
                return;
            }

            StopDrawing();
            
            _queuedCanvasOperation = new UndoLastDrawCanvasOperation(this);
            
            if (_remainingUndo > 0)
            {
                _remainingUndo--;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StartDrawing();
        }

        private void StartDrawing()
        {
            // Block drawing if Ctrl is being held
            if (InputUtility.IsCtrlHeld) return;
            if (IsUpdating) return;
            if(!CanDraw()) return;

            IsUpdating = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StopDrawing();
        }

        private void StopDrawing()
        {
            if (!IsUpdating) return;

            IsUpdating = false;
            _queuedCanvasOperation ??= new SnapshotCurrentCanvasOperation(this);
        }

        private void OnDestroy()
        {
            foreach (CanvasState canvasState in CanvasStateHistory)
            {
                canvasState.Destroy();
            }

            MainCanvasState.Destroy();
        }

        private void OnValidate()
        {
            RetrieveDependencies();
            RefreshCurrentBrushSettings();
        }

        private void RetrieveDependencies()
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
        
        private void InitializeUndoLimit()
        {
            if (RuntimeAsset == null || !RuntimeAsset.HasValue)
            {
                _remainingUndo = 0;
                return;
            }

            _remainingUndo = RuntimeAsset.UndoLimit;
        }
        
        private bool CanDraw()
        {
            return IsCanDraw && (!_drawingBoardController || _drawingBoardController.IsCanInteract);
        }
    }
}
