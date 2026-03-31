using UnityEngine;

public class CanvasSpawn : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private MainGameConfigRuntimeAsset mainGameConfigRuntimeAsset;
    
    private void Start()
    {
        SpawnCanvas();
    }
    
    private void SpawnCanvas()
    {
        MainGameModeConfig currentMainGameMode = mainGameConfigRuntimeAsset.Value;

        if (currentMainGameMode == null ||
            currentMainGameMode.CanvasTemplate == null ||
            currentMainGameMode.CanvasTemplate.Value == null)
        {
            Debug.LogWarning("No Canvas Template assigned!");
            return;
        }

        Instantiate(currentMainGameMode.CanvasTemplate.Value, transform);
    }
}
