 using UnityEngine;

namespace DaeHanKim.ThisIsTotallyADollar.Drawing
{
    [DisallowMultipleComponent]
    public class CanvasLayerDrawController : MonoBehaviour
    {
        static readonly int SHADER_PROPERTY_CANVASTEXTURE = Shader.PropertyToID("_CanvasTexture");
        static readonly int SHADER_PROPERTY_CLEARCOLOR = Shader.PropertyToID("_ClearColor");
        static readonly int SHADER_PROPERTY_BRUSHTEXTURE = Shader.PropertyToID("_BrushTexture");
        static readonly int SHADER_PROPERTY_BRUSHTEXTURESIZE = Shader.PropertyToID("_BrushTextureSize");
        static readonly int SHADER_PROPERTY_BRUSHCOLOR = Shader.PropertyToID("_BrushColor");
        static readonly int SHADER_PROPERTY_BRUSHSIZE = Shader.PropertyToID("_BrushSize");
        static readonly int SHADER_PROPERTY_PREVIOUSMOUSEPOSITION = Shader.PropertyToID("_PreviousTexelPosition");
        static readonly int SHADER_PROPERTY_CURRENTMOUSEPOSITION = Shader.PropertyToID("_CurrentTexelPosition");
        const string SHADER_KERNEL_INITIALIZE = "ClearCanvas";
        const string SHADER_KERNEL_DRAWUPDATE = "DrawUpdate";
        const string SHADER_KERNEL_ERASEUPDATE = "EraseUpdate";
        const int SHADER_NUMTHREADS_X = 8;
        const int SHADER_NUMTHREADS_Y = 8;

        [SerializeField] ComputeShader _drawComputeShader;
        [SerializeField] ComputeShader _clearComputeShader;

        public bool IsUpdating { get; private set; }

        // Layering
        RenderTexture[] _layerRenderTextures;
        RenderTexture _currentLayerRenderTexture;
        int? _currentLayerIndex;

        // Brush properties
        Texture _currentBrushTexture;
        Color? _currentBrushColor;
        float? _currentBrushSize;

        // Current drawing context
        Vector2? _previousTexelPosition;
        Vector2 _currentTexelPosition;

        // Compute shader
        int _csKernelInitializeIdx;
        int _csKernelDrawUpdateIdx;
        int _csKernelEraseUpdateIdx;

        public void Initialize(RenderTexture[] layerRenderTextures)
        {
            _layerRenderTextures = layerRenderTextures;
            _csKernelInitializeIdx = _clearComputeShader.FindKernel(SHADER_KERNEL_INITIALIZE);
            _csKernelDrawUpdateIdx = _drawComputeShader.FindKernel(SHADER_KERNEL_DRAWUPDATE);
            _csKernelEraseUpdateIdx = _drawComputeShader.FindKernel(SHADER_KERNEL_ERASEUPDATE);
            SetCurrentLayerIndex(0);
        }

        public void SetCurrentLayerIndex(int index)
        {
            if (_currentLayerIndex == index)
                return;

            _currentLayerIndex = index;
            _currentLayerRenderTexture = _layerRenderTextures[index];
            _clearComputeShader.SetTexture(_csKernelInitializeIdx, SHADER_PROPERTY_CANVASTEXTURE, _currentLayerRenderTexture);
            _drawComputeShader.SetTexture(_csKernelDrawUpdateIdx, SHADER_PROPERTY_CANVASTEXTURE, _currentLayerRenderTexture);
            _drawComputeShader.SetTexture(_csKernelEraseUpdateIdx, SHADER_PROPERTY_CANVASTEXTURE, _currentLayerRenderTexture);
        }

        public void ClearLayer(Color clearColor)
        {
            _clearComputeShader.SetVector(SHADER_PROPERTY_CLEARCOLOR, clearColor);
            DispatchComputeShader(_clearComputeShader, _csKernelInitializeIdx);
        }

        public void CopyTextureToCurrentLayer(Texture copyTexture)
        {
            Graphics.CopyTexture(copyTexture, _currentLayerRenderTexture);
        }

        public void Tick(bool update, CanvasDrawController.DrawMode drawMode, Vector2 texelPosition)
        {
            SetIsUpdating(update);

            if (!IsUpdating)
                return;

            int kernelIdx = drawMode == CanvasDrawController.DrawMode.Draw
                ? _csKernelDrawUpdateIdx
                : _csKernelEraseUpdateIdx;

            SetPreviousTexelPosition(_previousTexelPosition.HasValue ? _currentTexelPosition : texelPosition);
            SetCurrentTexelPosition(texelPosition);
            DispatchComputeShader(_drawComputeShader, kernelIdx);
        }

        void SetIsUpdating(bool draw)
        {
            if (draw && !IsUpdating)
            {
                StartDraw();
            }
            else if (!draw && IsUpdating)
            {
                StopDraw();
            }
        }

        void StartDraw()
        {
            IsUpdating = true;
        }

        void StopDraw()
        {
            _previousTexelPosition = null;
            IsUpdating = false;
        }

        void SetPreviousTexelPosition(Vector2 pos)
        {
            _previousTexelPosition = pos;
            _drawComputeShader.SetFloats(SHADER_PROPERTY_PREVIOUSMOUSEPOSITION, pos.x, pos.y);
        }

        void SetCurrentTexelPosition(Vector2 pos)
        {
            _currentTexelPosition = pos;
            _drawComputeShader.SetFloats(SHADER_PROPERTY_CURRENTMOUSEPOSITION, pos.x, pos.y);
        }

        public void SetBrushTexture(Texture tex)
        {
            if (tex == _currentBrushTexture)
                return;

            _currentBrushTexture = tex;
            _drawComputeShader.SetTexture(_csKernelDrawUpdateIdx, SHADER_PROPERTY_BRUSHTEXTURE, tex);
            _drawComputeShader.SetTexture(_csKernelEraseUpdateIdx, SHADER_PROPERTY_BRUSHTEXTURE, tex);
            _drawComputeShader.SetInts(SHADER_PROPERTY_BRUSHTEXTURESIZE, tex.width, tex.height);
        }

        public void SetBrushColor(Color color)
        {
            if (_currentBrushColor == color)
                return;

            _currentBrushColor = color;
            _drawComputeShader.SetVector(SHADER_PROPERTY_BRUSHCOLOR, color);
        }

        public void SetBrushSize(float size)
        {
            if (_currentBrushSize.HasValue && Mathf.Approximately(size, _currentBrushSize.Value))
                return;

            _currentBrushSize = size;
            _drawComputeShader.SetFloat(SHADER_PROPERTY_BRUSHSIZE, size);
        }

        void DispatchComputeShader(ComputeShader cs, int kernelIdx)
        {
            int threadGroupsX = _currentLayerRenderTexture.width / SHADER_NUMTHREADS_X;
            int threadGroupsY = _currentLayerRenderTexture.height / SHADER_NUMTHREADS_Y;
            cs.Dispatch(kernelIdx, threadGroupsX, threadGroupsY, 1);
        }
    }
}