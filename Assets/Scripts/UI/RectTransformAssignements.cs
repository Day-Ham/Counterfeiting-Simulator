using UnityEngine;

public class RectTransformAssignements : MonoBehaviour
{
    [SerializeField] private RectTransformValue value;
    
    [Header("Optional")]
    [Tooltip("Optional: Assign a GameObject manually if you want to register something other than this object.")]
    [SerializeField] private RectTransform overrideReference;
    
    private RectTransform RegisteredObject
    {
        get
        {
            // If override is set, use its RectTransform
            if (overrideReference != null)
            {
                return overrideReference.GetComponent<RectTransform>();
            }

            // Otherwise, use this object's RectTransform
            return GetComponent<RectTransform>();
        }
    }

    private void Awake()
    {
        if (value != null)
        {
            value.Set(RegisteredObject);
        }
    }

    private void OnDestroy()
    {
        if (value != null)
        {
            value.Clear(RegisteredObject);
        }
    }
}
