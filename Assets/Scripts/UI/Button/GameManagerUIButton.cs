using DaeHanKim.ThisIsTotallyADollar.Core;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUIButton : MonoBehaviour
{
    public Button Button;
    public GameManagerValue GameManagerValue;
    
    protected GameManager GameManager  => GameManagerValue.Value;
}
