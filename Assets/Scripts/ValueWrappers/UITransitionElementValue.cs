using UnityEngine;

[CreateAssetMenu(fileName = "UITransitionElementValue", menuName = "Value Wrapper/Scripts/UITransitionElement")]
public class UITransitionElementValue : ValueWrapper<UITransitionElement>
{
    public void MoveOutElement()
    {
        Value?.MoveOut();
    }

    public void MoveInElement()
    {
        Value?.MoveIn();
    }

    public void Bind(UITransitionElement element)
    {
        Value = element;
    }
}
