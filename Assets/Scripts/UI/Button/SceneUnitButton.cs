using UnityEngine;
using UnityEngine.UI;

public class SceneChangerUnitButton : MonoBehaviour
{
    public Button Button;
    public SceneChangerValue SceneChangerValue;

    protected SceneChanger SceneChanger => SceneChangerValue.Value;
}
