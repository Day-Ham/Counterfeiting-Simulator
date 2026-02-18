using UnityEngine;

public class RetryButton : SceneChangerUnitButton
{
    private void Awake()
    {
        Button.onClick.AddListener(RetryGame);
    }

    private void RetryGame()
    {
        SceneChanger.ResetScene();
    }
}
