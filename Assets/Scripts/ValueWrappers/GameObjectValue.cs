using UnityEngine;

[CreateAssetMenu(fileName = "NewGameObject", menuName = "Value Wrapper/GameObject")]
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
