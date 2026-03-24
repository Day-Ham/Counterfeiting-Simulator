using UnityEngine;

[CreateAssetMenu(fileName = "NewRectTransform", menuName = "Value Wrapper/RectTransform")]
public class RectTransformValue : ValueWrapper<RectTransform>
{
    public void Set(RectTransform rectTransform)
    {
        Value = rectTransform;
    }

    public void Clear(RectTransform rectTransform)
    {
        if (Value == rectTransform)
        {
            Value = null;
        }
    }
}
