using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace DaeHanKim.ThisIsTotallyADollar.Utility
{
    public class TextureUtility
    {
        static readonly int SHADER_PROPERTY_TEXTUREA = Shader.PropertyToID("_TextureA");
        static readonly int SHADER_PROPERTY_TEXTUREB = Shader.PropertyToID("_TextureB");
        static readonly int SHADER_PROPERTY_SIMILARITYOUTPUTBUFFER = Shader.PropertyToID("_SimilarityOutputBuffer");
        const string SHADER_KERNEL_SIMILARITY = "GetSimilarity";

        readonly ComputeShader _computeShader;
        readonly int _similarityKernelIndex;
        ComputeBuffer _similarityOutputBuffer;

        public TextureUtility(ComputeShader computeShader)
        {
            _computeShader = computeShader;
            _similarityKernelIndex = _computeShader.FindKernel(SHADER_KERNEL_SIMILARITY);
        }

        public void Create()
        {
            _similarityOutputBuffer = new ComputeBuffer(1, sizeof(int));
            _computeShader.SetBuffer(_similarityKernelIndex, SHADER_PROPERTY_SIMILARITYOUTPUTBUFFER, _similarityOutputBuffer);
        }

        public float? GetSimilarity(Texture textureA, Texture textureB)
        {
            if (textureA == null || textureB == null)
                return null;
            if (textureA.width != textureB.width)
                return null;
            if (textureA.height != textureB.height)
                return null;

            int[] outputData = new int[1];

            _similarityOutputBuffer.SetData(outputData);
            _computeShader.SetTexture(_similarityKernelIndex, SHADER_PROPERTY_TEXTUREA, textureA);
            _computeShader.SetTexture(_similarityKernelIndex, SHADER_PROPERTY_TEXTUREB, textureB);
            _computeShader.Dispatch(_similarityKernelIndex, textureA.width / 8, textureA.height / 8, 1);

            _similarityOutputBuffer.GetData(outputData);
            int similarPixelsCount = outputData[0];
            float overallSimilarity = (float) similarPixelsCount / (textureA.width * textureA.height);

            return overallSimilarity;
        }

        public Sprite CreateSpriteFromRenderTexture(RenderTexture renderTexture, Vector2 pivot)
        {
            // Only layer 0
            Vector2Int texSize = new(renderTexture.width, renderTexture.height);
            Texture2D finalTex = new(texSize.x, texSize.y, renderTexture.graphicsFormat, renderTexture.mipmapCount, TextureCreationFlags.None);
            Graphics.CopyTexture(renderTexture, finalTex);
            return Sprite.Create(finalTex, new Rect(Vector2.zero, texSize), pivot);
        }

        public void Destroy()
        {
            _similarityOutputBuffer.Release();
        }
    }
}