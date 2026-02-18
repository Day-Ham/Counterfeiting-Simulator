using UnityEngine;

[CreateAssetMenu(fileName = "New GameObject", menuName = "Value Wrapper/Game Object")]
public class GameObjectValue : ValueWrapper<GameObject>
{
    public void Set(GameObject go)
    {
        Value = go;
    }

    public void Clear(GameObject go)
    {
        if (Value == go)
        {
            Value = null;
        }
    }
}
