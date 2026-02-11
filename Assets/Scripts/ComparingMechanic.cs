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
    public TextMeshProUGUI percent;
    public Color PassedColor;
    public Color FailedColor;
    public GameObject ResetButton;
    public GameManager GameManager;
    
    private float x;
    private float y;
    
    [SerializeField] private int duration = 100;
    [SerializeField] private float threshold = 80;
    
    public Color InspectionColor;
    private bool OneShot = true;
    
    private void Start()
    {
        PercentageParent.transform.localScale = Vector3.zero;
        ObjectToMove2.GetComponent<RawImage>().color = Color.white;
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || !OneShot) return;
        
        GameManager.FinishGame();
        ObjectToMove.transform.DOMove(TargetLocation.transform.position, 1f, false).SetEase(EaseTween).OnComplete(
            () =>
            {
                StartCoroutine(ShowResult());
            });
        
        ObjectToMove2.GetComponent<RawImage>().color= InspectionColor;
        ObjectToMove2.transform.DOMove(TargetLocation.transform.position, 1f, false).SetEase(EaseTween);
        OneShot =false;
    }
    
    private IEnumerator ShowResult()
    {
        PercentageParent.transform.DOScale(Vector3.one, 1f).SetEase(PromptTween);
        
        for (int tick = 0; tick <= duration; tick++)
        { 
            yield return new WaitForSeconds(0.02f);
            x = Random.Range(1, 99);
            y = Random.Range(1, 99);
            percentageMajor.SetText(x.ToString());
            percentageMinor.SetText("."+ y.ToString());
            
            if (tick != duration) continue;
            
            percentageMajor.SetText(((GameManager.FirstTwoDigits).ToString("F0")));
            percentageMinor.SetText(("."+(GameManager.LastTwoDigits).ToString("F0")));
        }
        
        Debug.Log(GameManager.allSimilarity * 100);
        Debug.Log(threshold);
        Debug.Log(GameManager.allSimilarity * 100 > threshold);
        
        CheckingSimilar();
        
        yield return new WaitForSeconds(1f);
        
        Similar();
    }

    private void CheckingSimilar()
    {
        if (GameManager.allSimilarity * 100 < threshold)
        {
            percentageMajor.color = PassedColor;
            percentageMinor.color = PassedColor;
            percent.color = PassedColor;
        }
        else
        {
            percentageMajor.color = FailedColor;
            percentageMinor.color = FailedColor;
            percent.color = FailedColor;
        }
    }

    private void Similar()
    {
        if (GameManager.allSimilarity * 100 > threshold)
        {
            sceneChanger.ShowNextButton();
        }
        else
        {
            StartCoroutine(IndicateReset());
        }
    }

    private IEnumerator IndicateReset()
    {
        ResetButton.transform.DOScale(Vector3.one *1.25f, 1f).SetEase(PromptTween);
        
        yield return new WaitForSeconds(.75f);
        
        ResetButton.transform.DOScale(Vector3.one, 1f).SetEase(PromptTween);
        
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(IndicateReset());
    }
}
