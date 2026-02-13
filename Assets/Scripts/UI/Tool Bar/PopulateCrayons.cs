using UnityEngine;

public class PopulateCrayons : MonoBehaviour
{
    public ColorDataListValue ColorDataList;
    
    [SerializeField] private Transform ContentParent;
    [SerializeField] private GameObjectListValue ColorBlobVariants;
    [SerializeField] private SetCrayonFunction SetCrayonFunction;

    private void Start()
    {
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
        for (int i = 0; i < ColorDataList.Value.Count; i++)
        {
            GameObject randomPrefab = ColorBlobVariants.Value[Random.Range(0, ColorBlobVariants.Value.Count)];

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
