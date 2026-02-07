using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private UnityEvent ToggleEnable;
    [SerializeField] private UnityEvent ToggleDisable;

    private bool toggle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ApplyToggle()
    {
        toggle = !toggle;
        if (toggle)
        {
            ToggleEnable.Invoke();
        }
        else
        {
            ToggleDisable.Invoke();
        }
    }
}
