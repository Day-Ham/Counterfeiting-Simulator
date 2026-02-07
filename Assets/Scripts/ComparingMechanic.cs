using System.Collections;
using System.Collections.Generic;
using DaeHanKim.ThisIsTotallyADollar.Core;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class ComparingMechanic : MonoBehaviour
{
    public GameObject TargetLocation;

    public SceneChanger sceneChanger;
    public GameObject ObjectToMove;
    public GameObject ObjectToMove2;

    public Ease EaseTween;
    public Ease PromptTween;

    public GameObject PercentageParent;
    public TextMeshProUGUI percentageMajor;
    public TextMeshProUGUI percentageMinor;
    public TextMeshProUGUI percentageMajorShadow;
    public TextMeshProUGUI percentageMinorShadow;
    public TextMeshProUGUI percent;
    public TextMeshProUGUI percentShadow;
    public Color PassedColor;
    public Color PassedBGColor;
    public Color FailedColor;
    public Color FailedBGColor;
    public GameObject ResetButton;
    public GameManager gm;
    float x;
    float y;
    [SerializeField] int duration=100;
    [SerializeField] float threshold=80;
        public Color InspectionColor;
    private bool OneShot = true;
    // Start is called before the first frame update
    void Start()
    {
        PercentageParent.transform.localScale = Vector3.zero;
        ObjectToMove2.GetComponent<RawImage>().color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && OneShot)
        {
            gm.FinishGame();
            ObjectToMove.transform.DOMove(TargetLocation.transform.position, 1f, false).SetEase(EaseTween).OnComplete(
                () =>
                {
                    StartCoroutine(ShowResult());

                });
            ObjectToMove2.GetComponent<RawImage>().color= InspectionColor;
            ObjectToMove2.transform.DOMove(TargetLocation.transform.position, 1f, false).SetEase(EaseTween);
            OneShot =false;
        }
    }
    
    IEnumerator ShowResult()
    {
        PercentageParent.transform.DOScale(Vector3.one, 1f).SetEase(PromptTween);
        
        for (int tick =0;tick<= duration; tick++)
        { 
            yield return new WaitForSeconds(0.02f);
            x = Random.Range(1, 99);
            y = Random.Range(1, 99);
            percentageMajor.SetText(x.ToString());
            percentageMinor.SetText("."+ y.ToString());
            percentageMajorShadow.SetText(x.ToString());
            percentageMinorShadow.SetText("."+ y.ToString());
            if (tick == duration)
            {
                percentageMajor.SetText(((gm.FirstTwoDigits).ToString("F0")));
                percentageMinor.SetText(("."+(gm.LastTwoDigits).ToString("F0")));
                percentageMajorShadow.SetText(((gm.FirstTwoDigits).ToString("F0")));
                percentageMinorShadow.SetText(("."+(gm.LastTwoDigits).ToString("F0")));

               
            }
        }
        Debug.Log(gm.allSimilarity * 100);
        Debug.Log(threshold);
        Debug.Log(gm.allSimilarity * 100 > threshold);
        if (gm.allSimilarity * 100 < threshold)
        {
            percentageMajor.color = PassedColor;
            percentageMinor.color = PassedColor;
            percentageMajorShadow.color = PassedBGColor;
            percentageMinorShadow.color = PassedBGColor;
            percent.color = PassedColor;
            percentShadow.color = PassedBGColor;
        }
        else
        {
            percentageMajor.color = FailedColor;
            percentageMinor.color = FailedColor;
            percentageMajorShadow.color = FailedBGColor;
            percentageMinorShadow.color = FailedBGColor;
            percent.color = FailedColor;
            percentShadow.color = FailedBGColor;
        }
        yield return new WaitForSeconds(1f);
        if (gm.allSimilarity * 100 > threshold)
        {
            sceneChanger.ShowNextButton();
        }
        else
        {
            StartCoroutine(IndicateReset());
        }
    }

    IEnumerator IndicateReset()
    {
      
            ResetButton.transform.DOScale(Vector3.one *1.25f, 1f).SetEase(PromptTween);
            yield return new WaitForSeconds(.75f);
            ResetButton.transform.DOScale(Vector3.one, 1f).SetEase(PromptTween);
            yield return new WaitForSeconds(2f);
            StartCoroutine(IndicateReset());
    }
}
