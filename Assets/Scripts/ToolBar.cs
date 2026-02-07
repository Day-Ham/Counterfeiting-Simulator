using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ToolBar : MonoBehaviour
{   
    [SerializeField] private Color[] Palette; 
    [SerializeField] private ChangeColorButton[] Crayons;
    
    [SerializeField] private ToolItem SelectedItem;
    [SerializeField] private ToolItem[] Items;

    [SerializeField] private List<ToolItem> ToolsToReset;

    private void Awake()
    {
        for (int i = 0; i < Palette.Length; i++)
        {
            Crayons[i].DesiredColor = Palette[i];
        }
        
    }

    public void ResetPositions()
    {
        ToolsToReset.Clear();
        foreach (var item in Items)
        {
            if (SelectedItem != item)
            {
                ToolsToReset.Add(item);
            }
        }
        foreach (var item in ToolsToReset)
        {
            item.ResetPosition();
        }
    }

    public void TriggerReset(ToolItem selected)
    {
        SelectedItem = selected;
        ResetPositions();
    }

    private void OnValidate()
    {
        for (int i = 0; i < Palette.Length; i++)
        {
            Crayons[i].DesiredColor = Palette[i];
            Crayons[i].UpdateCrayon();
        }
    }
}
