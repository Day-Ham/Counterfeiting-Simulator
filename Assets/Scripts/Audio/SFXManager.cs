using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioClipEvent audioClipEvent;
    [SerializeField] private AudioSource audioSource;

    private void OnEnable() => audioClipEvent.Register(PlaySound);
    private void OnDisable() => audioClipEvent.Unregister(PlaySound);

    private void PlaySound(AudioClip clip) => audioSource.PlayOneShot(clip);
}
