using System;
using UnityEngine;

public enum UITransitionAction
{
    Enter,
    Exit
}

[Serializable]
public class UITransitionBinding
{
    public UITransitionAction action;
    public int batch;
}
