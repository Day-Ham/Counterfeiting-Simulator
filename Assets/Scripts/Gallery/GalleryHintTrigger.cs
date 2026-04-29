using UnityEngine;
using System.Collections;

public class GalleryHintTrigger : MonoBehaviour
{
    [SerializeField] private HintDataEvent hintDataEvent;
    [SerializeField] private HintData rightClickHint;
    [SerializeField] private float delay = 2f;

    private const string SeenKey = "GalleryRightClickHintSeen";

    private void Start()
    {
        if (ES3.Load(SeenKey, defaultValue: false)) return;
        StartCoroutine(ShowAfterDelay());
    }

    private IEnumerator ShowAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        hintDataEvent.Raise(rightClickHint);
        ES3.Save(SeenKey, true);
    }
}
