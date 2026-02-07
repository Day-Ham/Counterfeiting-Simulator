using UnityEngine;

namespace DaeHanKim.ThisIsTotallyADollar.Drawing
{
    public static class CanvasDrawUtilities
    {
        public static void CopyCanvasState(CanvasState srcCanvasState, CanvasState dstCanvasState)
        {
            for (int i = 0; i < srcCanvasState.LayersRenderTextures.Length; i++)
            {
                Texture srcTex = srcCanvasState.LayersRenderTextures[i];
                RenderTexture dstTex = dstCanvasState.LayersRenderTextures[i];
                Graphics.CopyTexture(srcTex, dstTex);
            }
        }
    }
}