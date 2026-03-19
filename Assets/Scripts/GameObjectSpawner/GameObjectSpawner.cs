using UnityEngine;

public class GameObjectSpawner : MonoBehaviour
{
    [Header("Prefabs to Instantiate")]
    [SerializeField] private GameObjectListValue _gameObjectListValueToSpawn;

    private void Start()
    {
        SpawnPrefabs();
    }
    
    private void SpawnPrefabs()
    {
        foreach (GameObject listGameObject in _gameObjectListValueToSpawn.Value)
        {
            Instantiate(listGameObject);
        }
    }
}
