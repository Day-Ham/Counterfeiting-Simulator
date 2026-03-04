using UnityEngine;
using UnityEngine.UI;

public class DrawingLoader : MonoBehaviour
{
    [Header("Output")]
    [SerializeField] private Image _targetSpriteRenderer;

    [Header("Settings")]
    [SerializeField] private Vector2 _pivot = new(0.5f, 0.5f);

    private const string DrawingKey = "Drawing";
    private const string LastFileKey = "LastDrawingFile";

    private void LoadDrawing(string fileName)
    {
        ES3Settings settings = new ES3Settings(fileName);

        if (!ES3.KeyExists(DrawingKey, settings))
        {
            Debug.LogWarning($"No drawing found in file: {fileName}");
            return;
        }

        byte[] pngBytes = ES3.Load<byte[]>(DrawingKey, settings);

        Texture2D loadedTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        loadedTexture.LoadImage(pngBytes);

        Sprite loadedSprite = Sprite.Create(
            loadedTexture,
            new Rect(0, 0, loadedTexture.width, loadedTexture.height),
            _pivot
        );

        SetSprite(loadedSprite);

        Debug.Log($"Loaded drawing from: {fileName}");
    }

    public void LoadLastDrawing()
    {
        if (!ES3.KeyExists(LastFileKey))
        {
            Debug.LogWarning("No last drawing file saved.");
            return;
        }

        string fileName = ES3.Load<string>(LastFileKey);
        LoadDrawing(fileName);
    }

    private void SetSprite(Sprite sprite)
    {
        if (_targetSpriteRenderer == null)
        {
            Debug.LogError("Target SpriteRenderer not assigned.");
            return;
        }

        // Prevent memory leak
        if (_targetSpriteRenderer.sprite != null)
        {
            Destroy(_targetSpriteRenderer.sprite.texture);
        }

        _targetSpriteRenderer.sprite = sprite;
    }
}
