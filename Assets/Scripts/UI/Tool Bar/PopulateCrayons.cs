using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PopulateCrayons : MonoBehaviour
{
    [Header("Crayon Data")]
    [SerializeField] private GameModeCrayon _crayonSource; // Assign via inspector

    [Header("UI References")]
    [SerializeField] private Transform _contentParent;
    [SerializeField] private SetCrayonFunction _setCrayonFunction;

    private void Start()
    {
        if (_crayonSource == null)
        {
            Debug.LogError("No crayon source assigned!");
            return;
        }

        _crayonSource.InitializeColors();

        Populate();

        Setup();
    }
    
    private void Setup()
    {
        _setCrayonFunction.SetupCrayons(_crayonSource.GetActiveColors());
    }

    private void Populate()
    {
        ClearChildren();
        SpawnDifferentBlobs();
    }

    private void SpawnDifferentBlobs()
    {
        var colors = _crayonSource.GetActiveColors();
        var blobs = _crayonSource.GetColorBlobs().Value;

        int prefabCount = blobs.Count;
        for (int i = 0; i < colors.Count; i++)
        {
            if (prefabCount == 0) break;

            GameObject prefab = blobs[Random.Range(0, prefabCount)];
            GameObject instance = Instantiate(prefab, _contentParent);

            if (instance.TryGetComponent<CrayonUIItem>(out var crayonItem))
            {
                crayonItem.Setup(colors[i], i);
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
