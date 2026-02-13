using UnityEngine;

public class PopulateCrayons : MonoBehaviour
{
    [SerializeField] private LevelConfigRuntimeAsset LevelConfigRuntimeSO;
    [SerializeField] private Transform ContentParent;
    [SerializeField] private SetCrayonFunction SetCrayonFunction;
    
    public ColorDataListValue ColorDataList { get; private set; }
    private GameObjectListValue _currentBlobs;
    
    private void OnEnable()
    {
        LevelConfigRuntimeSO.OnValueChanged += OnLevelConfigChanged;
    }

    private void OnDisable()
    {
        LevelConfigRuntimeSO.OnValueChanged -= OnLevelConfigChanged;
    }

    private void Start()
    {
        if (LevelConfigRuntimeSO.Value != null)
        {
            OnLevelConfigChanged(LevelConfigRuntimeSO.Value);
        }
    }
    
    private void OnLevelConfigChanged(LevelConfig newConfig)
    {
        if (newConfig == null) return;

        ColorDataList = newConfig.LevelColors;
        _currentBlobs = newConfig.LevelColorBlobs;

        Populate();
        SetCrayonFunction.SetupCrayons();
    }

    private void Populate()
    {
        ClearChildren();
        
        SpawnDifferentBlobs();
    }

    private void SpawnDifferentBlobs()
    {
        if (_currentBlobs == null) return;

        for (int i = 0; i < ColorDataList.Value.Count; i++)
        {
            GameObject randomPrefab = _currentBlobs.Value[Random.Range(0, _currentBlobs.Value.Count)];
            Instantiate(randomPrefab, ContentParent);
        }
    }

    private void ClearChildren()
    {
        for (int i = ContentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(ContentParent.GetChild(i).gameObject);
        }
    }
}
