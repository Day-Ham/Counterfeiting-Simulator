using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DaeHanKim.ThisIsTotallyADollar.Drawing
{
    [DisallowMultipleComponent]
    public class CanvasDrawView : MonoBehaviour
    {
        readonly List<RawImage> _rawImages = new();

        public void SetImageTextures(params RenderTexture[] layerTextures)
        {
            for (int i = _rawImages.Count; i < layerTextures.Length; i++)
            {
                GameObject rawImageGo = new($"Layer_{i}", typeof(RectTransform));
                RectTransform rawImageRectTransform = (RectTransform) rawImageGo.transform;
                rawImageRectTransform.SetParent(transform, false);
                rawImageRectTransform.anchorMin = Vector2.zero;
                rawImageRectTransform.anchorMax = Vector2.one;
                rawImageRectTransform.anchoredPosition = Vector2.zero;
                rawImageRectTransform.sizeDelta = Vector2.zero;

                RawImage rawImage = rawImageGo.AddComponent<RawImage>();
                _rawImages.Add(rawImage);

                Texture tex = layerTextures[i];
                rawImage.texture = tex;
            }

            for (int i = layerTextures.Length; i < _rawImages.Count; i++)
            {
                Destroy(_rawImages[i].gameObject);
            }

            _rawImages.RemoveRange(layerTextures.Length, _rawImages.Count - layerTextures.Length);
        }
    }
}