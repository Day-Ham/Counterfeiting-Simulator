using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ColorChangeManager : MonoBehaviour
{
    [SerializeField] private List<ChangeColor> colorChangeObjects = new List<ChangeColor>();
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float moveInterval = 1f;

    private ChangeColor currentlySelected = null;
    private Dictionary<ChangeColor, Vector2> originalPositions = new Dictionary<ChangeColor, Vector2>();

    void Start()
    {
        foreach (ChangeColor obj in colorChangeObjects)
        {
            if (obj != null)
            {
                RectTransform rect = obj.GetComponent<RectTransform>();
                if (rect != null)
                {
                    originalPositions[obj] = rect.anchoredPosition;
                }
            }
        }

        StartCoroutine(MoveToUp());
    }

    IEnumerator MoveToUp()
    {
        while (true)
        {
            if (currentlySelected != null)
            {
                ResetPosition(currentlySelected);
            }

            yield return new WaitForSeconds(moveInterval);

            if (colorChangeObjects.Count > 0)
            {
                int randomIndex = Random.Range(0, colorChangeObjects.Count);
                currentlySelected = colorChangeObjects[randomIndex];

                MoveUpByTen(currentlySelected);
            }

            yield return new WaitForSeconds(moveDuration);
        }
    }

    void MoveUpByTen(ChangeColor obj)
    {
        if (obj != null)
        {
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            if (rectTransform != null && originalPositions.ContainsKey(obj))
            {
                rectTransform.DOAnchorPos(originalPositions[obj] + Vector2.up * 10, moveDuration)
                    .SetEase(Ease.OutBounce);
            }
        }
    }

    void ResetPosition(ChangeColor obj)
    {
        if (obj != null)
        {
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            if (rectTransform != null && originalPositions.ContainsKey(obj))
            {
                rectTransform.DOAnchorPos(originalPositions[obj], 0.3f)
                    .SetEase(Ease.InOutQuad);
            }
        }
    }
}