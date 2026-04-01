using UnityEngine;

public class GridLinesController : MonoBehaviour
{
    [SerializeField] private GameObjectValue[] gridObjects;
    [SerializeField] private BoolEvent boolEvent;

    private void OnEnable()
    {
        boolEvent.Register(SetGridActive);
    }

    private void OnDisable()
    {
        boolEvent.Unregister(SetGridActive);
    }

    private void SetGridActive(bool active)
    {
        foreach (var gridGameObject in gridObjects)
        {
            if (gridGameObject?.Value)
            {
                gridGameObject.Value.SetActive(active);
            }
        }
    }
}
