using UnityEngine;

public class GameObjectsAssignements : MonoBehaviour
{
    [SerializeField] private GameObjectValue _value;

    private void Awake()
    {
        if (_value != null)
        {
            _value.Value = gameObject;
        }
    }

    private void OnDisable()
    {
        if (_value != null && _value.Value == gameObject)
        {
            _value.Value = null;
        }
    }
}
