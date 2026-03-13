using UnityEngine;
using UnityEngine.UI;

public class SceneChangerUnitButton : MonoBehaviour
{
    public Button Button;
    public LevelChangerValue levelChangerValue;

    protected LevelChanger LevelChanger => levelChangerValue.Value;
}
