using UnityEngine;

public class QuitButton : SceneChangerUnitButton
{
    private void Awake()
    {
        Button.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        SceneChanger.QuitApp();
    }
}
