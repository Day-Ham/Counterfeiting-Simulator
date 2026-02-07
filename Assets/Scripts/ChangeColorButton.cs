using System;
using System.Collections;
using System.Collections.Generic;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;
using DaeHanKim.Utilities;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ChangeColorButton : MonoBehaviour
{

    public CanvasLayerDrawController Controller;
    public CanvasDrawController DrawController;
    public Color DesiredColor;
    public Image render;
    public bool Eraser=false;

    private void Start()
    {
        UpdateCrayon();
    }

    public void SetPaintColor()
    {
        Controller.SetBrushColor(DesiredColor);
        DrawController.SetDrawMode(CanvasDrawController.DrawMode.Draw);
        
    }
    
    public void Erase()
    {
        DrawController.SetDrawMode(CanvasDrawController.DrawMode.Erase);
    }

    public void UpdateCrayon()
    {
        if(!Eraser)
            render.color = DesiredColor;
    }
}
