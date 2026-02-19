using UnityEngine;

public class GameObjectsAssignements : MonoBehaviour
{
    [SerializeField] private GameObjectValue _value;
    
    [Header("Optional")]
    [Tooltip("Optional: Assign a GameObject manually if you want to register something other than this object.")]
    [SerializeField] private GameObject _overrideReference;
    
    private GameObject _registeredObject => _overrideReference != null ? _overrideReference : gameObject;

    private void Awake()
    {
        if (_value != null)
        {
            _value.Set(_registeredObject);
        }
    }

    private void OnDestroy()
    {
        if (_value != null)
        {
            _value.Clear(_registeredObject);
        }
    }
}
