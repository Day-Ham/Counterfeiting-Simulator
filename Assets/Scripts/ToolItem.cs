using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ToolItem : MonoBehaviour
{
    public Animator anim;
    public ToolBar bar;
    


    public void ResetPosition()
    {
        anim.Play("Normal");
    }

    public void IndicatePosition()
    {
        anim.Play("Selected");
    }

    public void SetSelected()
    {
        bar.TriggerReset(this);
    }
}
