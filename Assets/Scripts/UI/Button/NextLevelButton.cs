using UnityEngine;

public class NextLevelButton : SceneChangerUnitButton
{
    private void Awake()
    {
        Button.onClick.AddListener(NextLevel);
    }

    private void NextLevel()
    {
        LevelChanger.NextLevel();
    }
}
