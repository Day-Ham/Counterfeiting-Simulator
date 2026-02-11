using UnityEngine;

public class GameObjectsAssignements : MonoBehaviour
{
    public GameObject GameObjectTarget;
    
    public GameObjectValue GameObjectValue;

    private void Awake()
    {
        GameObjectValue.Value = GameObjectTarget;
    }
}
