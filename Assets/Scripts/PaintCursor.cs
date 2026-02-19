using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PaintCursor : MonoBehaviour
{
    [Header("Runtime References")]
    public LevelConfigRuntimeAsset RuntimeCanvas;
    public RawImage[] mouseCursors;

    private static Canvas _canvas;
    private static Camera _canvasCamera;
    private RectTransform[] _cursorRects;

    private void Awake()
    {
        if (mouseCursors == null || mouseCursors.Length <= 0)
        {
            return;
        }
        
        _cursorRects = new RectTransform[mouseCursors.Length];

        for (int i = 0; i < mouseCursors.Length; i++)
        {
            if (mouseCursors[i] != null)
            {
                _cursorRects[i] = mouseCursors[i].rectTransform;
            }
        }
    }

    private void Start()
    {
        Cursor.visible = false;

        if (RuntimeCanvas.Value == null)
        {
            return;
        }

        _canvas = RuntimeCanvas.Value.CanvasTemplate.Value;

        if (_canvas != null)
        {
            _canvasCamera = GetCanvasCamera(_canvas);
        };
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            Input.mousePosition,
            _canvasCamera,
            out var localPoint
        );

        foreach (var rect in _cursorRects)
        {
            if (rect)
            {
                rect.anchoredPosition = localPoint;
            }
        }
    }
    
    private Camera GetCanvasCamera(Canvas canvas)
    {
        return canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null;
    }
}
