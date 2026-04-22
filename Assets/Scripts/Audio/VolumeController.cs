using UnityEngine;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PersistentFloatValue volume;

    private void OnEnable()
    {
        audioSource.volume = volume.Value;
        volume.OnValueChanged += SetVolume;
    }
    
    private void OnDisable() => volume.OnValueChanged -= SetVolume;

    private void SetVolume(float value) => audioSource.volume = value;
}
