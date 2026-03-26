using SFB;
using UnityEngine;
using UnityEngine.UI;

public class DownloadController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private ByteArrayEvent selectedImageEvent;
    [SerializeField] private BoolEvent openDownloadUIEvent;
    
    [Header("UI")]
    [SerializeField] private Button downloadButton;
    
    private byte[] _currentImageData;

    private void OnEnable()
    {
        downloadButton.onClick.AddListener(SaveCurrentImage);
        selectedImageEvent.Register(SetCurrentImage);
    }

    private void OnDisable()
    {
        downloadButton.onClick.RemoveListener(SaveCurrentImage);
        selectedImageEvent.Unregister(SetCurrentImage);
    }

    private void SetCurrentImage(byte[] imageData)
    {
        _currentImageData = imageData;
    }

    private void SaveCurrentImage()
    {
        if (_currentImageData == null || _currentImageData.Length == 0) return;

        string path = StandaloneFileBrowser.SaveFilePanel(
            "Save Image As",
            "",
            "Painting.png",
            "png"
        );

        if (string.IsNullOrEmpty(path)) return;
        
        System.IO.File.WriteAllBytes(path, _currentImageData);
        Debug.Log($"Image saved to {path}");
        openDownloadUIEvent.Raise(false);
    }
}
