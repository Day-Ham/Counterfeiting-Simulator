using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class DrawingGalleryManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform _contentParent;
    [SerializeField] private GalleryItemUI _galleryItemPrefab;
    [SerializeField] private Image _previewImage;

    [Header("Settings")]
    [SerializeField] private Vector2 _pivot = new(0.5f, 0.5f);

    private const string DrawingKey = "Drawing";

    private void Start()
    {
        LoadGallery();
    }

    public void LoadGallery()
    {
        string path = Application.persistentDataPath;

        var files = Directory.GetFiles(path, "Drawing_*.es3");

        foreach (var filePath in files.OrderByDescending(f => f))
        {
            string fileName = Path.GetFileName(filePath);

            Sprite thumbnail = LoadSpriteFromFile(fileName);

            if (thumbnail == null)
                continue;

            GalleryItemUI item = Instantiate(_galleryItemPrefab, _contentParent);
            item.Initialize(fileName, thumbnail, OnGalleryItemClicked);
        }
    }

    private Sprite LoadSpriteFromFile(string fileName)
    {
        ES3Settings settings = new ES3Settings(fileName);

        if (!ES3.KeyExists(DrawingKey, settings))
            return null;

        byte[] pngBytes = ES3.Load<byte[]>(DrawingKey, settings);

        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(pngBytes);

        return Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            _pivot
        );
    }

    private void OnGalleryItemClicked(string fileName)
    {
        Sprite sprite = LoadSpriteFromFile(fileName);

        if (_previewImage.sprite != null)
        {
            Destroy(_previewImage.sprite.texture);
        }

        _previewImage.sprite = sprite;
        _previewImage.preserveAspect = true;

        Debug.Log($"Loaded preview: {fileName}");
    }
}
