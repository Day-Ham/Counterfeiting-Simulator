using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class UITransitionTweenSettings
{
    [Header("Timing")]
    public float enterDuration = 0.5f;
    public float exitDuration = 0.5f;

    [Header("Delay")]
    public float enterDelay;
    public float exitDelay;

    [Header("Ease")]
    public Ease enterEase = Ease.OutQuad;
    public Ease exitEase = Ease.InQuad;
}
