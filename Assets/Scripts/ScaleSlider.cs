using System.Collections;
using System.Collections.Generic;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;
using UnityEngine.UI;
public class ScaleSlider : MonoBehaviour
{
    public Slider slider;
    public CanvasDrawController brushController;
    public GameObject cursor;

    public float ReferenceNumber = 64;
    // Start is called before the first frame update
    void Start()
    {
        brushController.SetBrushSize(slider.value * ReferenceNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSize()
    {
        cursor.transform.localScale = new Vector3(slider.value, slider.value, slider.value);
        brushController.SetBrushSize(slider.value * ReferenceNumber);
    }
}
