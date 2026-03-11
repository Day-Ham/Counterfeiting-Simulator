using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PopulateCrayons : MonoBehaviour
{
    [Header("Crayon Data")]
    [SerializeField] private ScriptableObject _crayonSourceSO; // Either LevelConfig or SandboxModeConfig

    private ICrayonSource _crayonSource;

    [Header("UI References")]
    [SerializeField] private Transform _contentParent;
    [SerializeField] private SetCrayonFunction _setCrayonFunction;
    
    private void Start()
    {
        // Detect which adapter to use
        if (_crayonSourceSO is LevelConfigRuntimeAsset lvlAsset)
        {
            _crayonSource = new LevelConfigCrayonSource(lvlAsset);
        }
        else if (_crayonSourceSO is SandboxModeConfig sandbox)
        {
            _crayonSource = new SandboxCrayonSource(sandbox);
        }
        else
        {
            Debug.LogError("Unsupported crayon source!");
            return;
        }

        _crayonSource.InitializeColors();

        Populate();
        Setup();
    }
    
    private void Setup() => _setCrayonFunction.SetupCrayons(_crayonSource.GetActiveColors());

    private void Populate()
    {
        ClearChildren();
        SpawnDifferentBlobs();
    }

    private void SpawnDifferentBlobs()
    {
        var colorsList = _crayonSource.GetActiveColors();
        var colorBlobsList = _crayonSource.GetColorBlobs().Value;

        int colorCount = colorsList.Count;
        int prefabCount = colorBlobsList.Count;

        for (int i = 0; i < colorCount; i++)
        {
            if (prefabCount == 0) break;

            GameObject randomPrefab = colorBlobsList[Random.Range(0, prefabCount)];
            GameObject instance = Instantiate(randomPrefab, _contentParent);

            if (instance.TryGetComponent<CrayonUIItem>(out var crayonItem))
            {
                crayonItem.Setup(colorsList[i], i);
            }
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
