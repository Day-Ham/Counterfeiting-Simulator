using System;
//using Sirenix.OdinInspector;
using UnityEngine;

namespace DaeHanKim.ThisIsTotallyADollar.Drawing
{
    public class CanvasStamper : MonoBehaviour
    {
        static readonly int SHADER_PROPERTY_CANVASTEXTURE = Shader.PropertyToID("_CanvasTexture");
        static readonly int SHADER_PROPERTY_BRUSHTEXTURE = Shader.PropertyToID("_BrushTexture");
        static readonly int SHADER_PROPERTY_BRUSHTEXTURESIZE = Shader.PropertyToID("_BrushTextureSize");
        static readonly int SHADER_PROPERTY_BRUSHCOLOR = Shader.PropertyToID("_BrushColor");
        static readonly int SHADER_PROPERTY_BRUSHSIZE = Shader.PropertyToID("_BrushSize");
        static readonly int SHADER_PROPERTY_PREVIOUSMOUSEPOSITION = Shader.PropertyToID("_PreviousTexelPosition");
        static readonly int SHADER_PROPERTY_CURRENTMOUSEPOSITION = Shader.PropertyToID("_CurrentTexelPosition");
        const string SHADER_KERNEL_DRAWUPDATE = "DrawUpdate";
        const int SHADER_NUMTHREADS_X = 8;
        const int SHADER_NUMTHREADS_Y = 8;

        [SerializeField] ComputeShader _drawComputeShader;

        int _csKernelDrawUpdateIdx;

        void Awake()
        {
            _drawComputeShader = Instantiate(_drawComputeShader); // Duplicate it just to make sure it's unique
            _csKernelDrawUpdateIdx = _drawComputeShader.FindKernel(SHADER_KERNEL_DRAWUPDATE);
        }

        void OnDestroy()
        {
            if (_drawComputeShader != null)
            {
                Destroy(_drawComputeShader);
            }
        }

        public void StampTexture(RenderTexture target, Texture brushTexture, Color brushColor, float brushSize, Vector2 texelPosition)
        {
            _drawComputeShader.SetTexture(_csKernelDrawUpdateIdx, SHADER_PROPERTY_CANVASTEXTURE, target);
            _drawComputeShader.SetTexture(_csKernelDrawUpdateIdx, SHADER_PROPERTY_BRUSHTEXTURE, brushTexture);
            _drawComputeShader.SetInts(SHADER_PROPERTY_BRUSHTEXTURESIZE, brushTexture.width, brushTexture.height);
            _drawComputeShader.SetVector(SHADER_PROPERTY_BRUSHCOLOR, brushColor);
            _drawComputeShader.SetFloat(SHADER_PROPERTY_BRUSHSIZE, brushSize);
            _drawComputeShader.SetFloats(SHADER_PROPERTY_PREVIOUSMOUSEPOSITION, texelPosition.x, texelPosition.y);
            _drawComputeShader.SetFloats(SHADER_PROPERTY_CURRENTMOUSEPOSITION, texelPosition.x, texelPosition.y);
            int threadGroupsX = target.width / SHADER_NUMTHREADS_X;
            int threadGroupsY = target.height / SHADER_NUMTHREADS_Y;
            _drawComputeShader.Dispatch(_csKernelDrawUpdateIdx, threadGroupsX, threadGroupsY, 1);
        }
    }
}