using UnityEngine;

public class PopulatCrayons : MonoBehaviour
{
    [SerializeField] private ColorDataListValue ColorDataList;
    [SerializeField] private Transform ContentParent;
    [SerializeField] private GameObjectListValue ColorBlobVariants;

    private void Start()
    {
        Populate();
    }

    private void Populate()
    {
        ClearChildren();
        
        SpawnDifferentBlobs();
    }

    private void SpawnDifferentBlobs()
    {
        foreach (var color in ColorDataList.Value)
        {
            GameObject randomPrefab = ColorBlobVariants.Value[Random.Range(0, ColorBlobVariants.Value.Count)];

            GameObject instance = Instantiate(randomPrefab, ContentParent);

            CrayonUIItem uiItem = instance.GetComponent<CrayonUIItem>();
            
            if (uiItem != null)
            {
                uiItem.Setup(color);
            }
            else
            {
                Debug.LogWarning("CrayonUIItem missing on prefab", instance);
            }
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
