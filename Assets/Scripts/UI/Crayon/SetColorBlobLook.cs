using UnityEngine;
using UnityEngine.UI;

public class SetColorBlobLook : MonoBehaviour
{
    [SerializeField] private SpriteListValue blobLooksList;
    [SerializeField] private Image colorBlobShadow;
    [SerializeField] private Image colorBlobMain;
    
    public Image ColorBlobShadow => colorBlobShadow;

    private void Start()
    {
        ApplyRandomLook();
    }

    private void ApplyRandomLook()
    {
        // Safety checks
        if (blobLooksList == null || blobLooksList.Value == null || blobLooksList.Value.Count == 0)
        {
            Debug.LogWarning("Blob Looks List is empty or not assigned!");
            return;
        }

        int randomIndex = Random.Range(0, blobLooksList.Value.Count);
        var selectedSprite = blobLooksList.Value[randomIndex];

        if (selectedSprite == null)
        {
            Debug.LogWarning("Selected sprite is null!");
            return;
        }

        colorBlobShadow.sprite = selectedSprite;
        colorBlobMain.sprite = selectedSprite;
    }
    
    public void SetShadowColor(Color color)
    {
        if (colorBlobShadow != null)
        {
            colorBlobShadow.color = color;
        };
    }
}
