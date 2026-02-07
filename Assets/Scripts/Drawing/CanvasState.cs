using UnityEngine;

namespace DaeHanKim.ThisIsTotallyADollar.Drawing
{
    public class CanvasState
    {
        public readonly RenderTexture[] LayersRenderTextures;

        public CanvasState(RenderTexture[] layersRenderTextures)
        {
            LayersRenderTextures = layersRenderTextures;
        }

        public void Destroy()
        {
            foreach (RenderTexture tex in LayersRenderTextures)
            {
                if (tex.IsCreated())
                {
                    tex.Release();
                }
            }
        }
    }
}