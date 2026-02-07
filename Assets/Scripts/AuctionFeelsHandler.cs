using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class AuctionFeelsHandler : MonoBehaviour
{

    public GameObject TargetLocation;
    public GameObject TargetToolbarLocation;

    public GameObject Painting;
    public GameObject Toolbar;
    public GameObject Frame;
    public GameObject FrameShadow;


    public Ease EaseTween;
    private bool OneShot = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && OneShot)
        {

            Painting.transform.DOMove(TargetLocation.transform.position, 1f, false).SetEase(EaseTween).OnComplete(
                () =>
                {
                    Frame.transform.DOMove(TargetLocation.transform.position, 1f, false).SetEase(EaseTween);
                    FrameShadow.transform.DOMove(TargetLocation.transform.position, 1f, false).SetEase(EaseTween).OnComplete(
                    () =>
                    {
                        Toolbar.transform.DOMove(TargetToolbarLocation.transform.position, 1f, false).SetEase(EaseTween);
                    });
                });
            OneShot = false;
        }
    }
}
