using UnityEngine;

public class MasterVolumeController : MonoBehaviour
{
    [SerializeField] private PersistentFloatValue masterVolume;

    private void OnEnable()
    {
        AudioListener.volume = masterVolume.Value;
        masterVolume.OnValueChanged += SetMasterVolume;
    }
    
    private void OnDisable() => masterVolume.OnValueChanged -= SetMasterVolume;

    private void SetMasterVolume(float value) => AudioListener.volume = value;
}
