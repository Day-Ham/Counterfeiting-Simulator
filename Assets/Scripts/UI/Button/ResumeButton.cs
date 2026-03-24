using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnResumeClicked);
    }

    private void OnResumeClicked()
    {
        GameState.ResumeGame(); // 🔥 direct
    }
}
