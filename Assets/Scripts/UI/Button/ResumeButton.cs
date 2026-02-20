using UnityEngine;

public class ResumeButton : GameManagerUIButton
{
    public GameObject PauseUIGameObject;
    
    private void Awake()
    {
        Button.onClick.AddListener(ResumeGame);
    }

    private void ResumeGame()
    {
        PauseUIGameObject.SetActive(false);
        GameManager.ResumeGame();
    }
}
