using UnityEngine;

public class PopulatCrayons : MonoBehaviour
{
    [SerializeField] private ColorDataListValue ColorDataList;
    [SerializeField] private Transform ContentParent;
    [SerializeField] private GameObjectValue CrayonUIPrefab;

    private void Start()
    {
        Populate();
    }

    private void Populate()
    {
        ClearChildren();
        
        foreach (var color in ColorDataList.Value)
        {
            GameObject instance = Instantiate(CrayonUIPrefab.Value, ContentParent);
            
            CrayonUIItem uiItem = instance.GetComponent<CrayonUIItem>();

            uiItem.Setup(color);
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
