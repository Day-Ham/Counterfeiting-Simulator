using UnityEngine;

public class CanvasSpawn : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntimeAsset;
    
    private void Start()
    {
        SpawnCanvas();
    }
    
    private void SpawnCanvas()
    {
        LevelConfig currentLevel = _levelConfigRuntimeAsset.Value;

        if (currentLevel == null ||
            currentLevel.CanvasTemplate == null ||
            currentLevel.CanvasTemplate.Value == null)
        {
            Debug.LogWarning("No Canvas Template assigned!");
            return;
        }

        Instantiate(currentLevel.CanvasTemplate.Value, transform);
    }
}
