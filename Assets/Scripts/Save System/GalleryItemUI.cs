using System;
using UnityEngine;
using UnityEngine.UI;

public class GalleryItemUI : MonoBehaviour
{
    [SerializeField] private Image _thumbnail;
    private string _fileName;
    private Action<string> _onClick;

    public void Initialize(string fileName, Sprite sprite, Action<string> onClick)
    {
        _fileName = fileName;
        _thumbnail.sprite = sprite;
        _thumbnail.preserveAspect = true;
        _onClick = onClick;
    }

    public void OnClick()
    {
        _onClick?.Invoke(_fileName);
    }
}
