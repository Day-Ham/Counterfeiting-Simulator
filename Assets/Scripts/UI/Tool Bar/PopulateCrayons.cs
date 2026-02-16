using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PopulateCrayons : MonoBehaviour
{
    [Header("Crayon Data")]
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntimeAsset;

    [Header("UI References")]
    [SerializeField] private Transform _contentParent;
    [SerializeField] private SetCrayonFunction _setCrayonFunction;
    
    private LevelConfig _currentLevel;

    private void Start()
    {
        _currentLevel = _levelConfigRuntimeAsset.Value;
        
        Populate();
        _setCrayonFunction.SetupCrayons(_currentLevel.LevelColors);
    }

    private void Populate()
    {
        ClearChildren();
        SpawnDifferentBlobs();
    }

    private void SpawnDifferentBlobs()
    {
        var colorsList = _currentLevel.LevelColors.Value;
        var colorBlobsList = _currentLevel.LevelColorBlobs.Value;

        int colorCount = colorsList.Count;
        int prefabCount = colorBlobsList.Count;

        for (int i = 0; i < colorCount; i++)
        {
            if (prefabCount == 0) break;

            GameObject randomPrefab = colorBlobsList[Random.Range(0, prefabCount)];
            Instantiate(randomPrefab, _contentParent);
        }
    }

    private void ClearChildren()
    {
        for (int i = _contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(_contentParent.GetChild(i).gameObject);
        }
    }
}
