using DaeHanKim.ThisIsTotallyADollar.Core;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : GameManagerUIButton
{
    private void Awake()
    {
        Button.onClick.AddListener(PauseGame);
    }

    private void PauseGame()
    {
        GameManager.PauseGame();
    }
}
