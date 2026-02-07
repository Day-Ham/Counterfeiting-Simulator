using System.Collections;
using System.Collections.Generic;
using DaeHanKim.ThisIsTotallyADollar.Core;
using UnityEngine;
using UnityEngine.UI;

public class LevelSetter : MonoBehaviour
{

    public Texture newSprite;
    public RawImage OriginalPic;

    public RawImage FrontPic;

    // Start is called before the first frame update
    void OnValidate()
    {
        OriginalPic.texture = newSprite;
        FrontPic.texture = newSprite;
    }
}
