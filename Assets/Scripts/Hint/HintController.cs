using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HintController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private HintDataEvent hintDataEvent;
    [SerializeField] private VoidEvent hintDismissedEvent;
    [SerializeField] private MainGameConfigRuntimeAsset _runtimeAsset;

    [Header("General Hints")]
    [SerializeField] private HintDataListValue generalHints;

    private bool _hintDismissed;

    private void OnEnable()
    {
        GameState.OnGameStarted += StartHints;
        GameState.OnGameFinished += StopHints;
        hintDismissedEvent.Register(OnHintDismissed);
    }

    private void OnDisable()
    {
        GameState.OnGameStarted -= StartHints;
        GameState.OnGameFinished -= StopHints;
        hintDismissedEvent.Unregister(OnHintDismissed);
    }

    private void OnHintDismissed() => _hintDismissed = true;

    private void StartHints()
    {
        StartCoroutine(HintsRoutine());
    }

    private void StopHints()
    {
        StopAllCoroutines();
    }

    private IEnumerator HintsRoutine()
    {
        var levelHints = _runtimeAsset?.Value?.Hints?.Value;

        if (levelHints != null)
        {
            foreach (var hint in levelHints)
            {
                yield return new WaitForSeconds(hint.delay);
                _hintDismissed = false;
                hintDataEvent.Raise(hint);
                yield return new WaitUntil(() => _hintDismissed);
            }
        }

        yield return StartCoroutine(GeneralHintsRoutine());
    }

    private IEnumerator GeneralHintsRoutine()
    {
        if (generalHints?.Value == null || generalHints.Value.Count == 0) yield break;

        List<HintData> remaining = new List<HintData>(generalHints.Value);

        while (remaining.Count > 0)
        {
            int randomIndex = Random.Range(0, remaining.Count);
            HintData hint = remaining[randomIndex];
            remaining.RemoveAt(randomIndex);

            yield return new WaitForSeconds(hint.delay);
            _hintDismissed = false;
            hintDataEvent.Raise(hint);

            yield return new WaitUntil(() => _hintDismissed);
        }
    }
}
