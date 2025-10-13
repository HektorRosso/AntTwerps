using UnityEngine;

public class ScreamSFX : MonoBehaviour
{
    [Tooltip("Fall Settings")]
    public float fallThresholdY = -5f;

    [Tooltip("AudioSource")]
    public AudioSource audioSource;

    [Tooltip("SFX")]
    public AudioClip screamClip;

    [Tooltip("Pitch")]
    public float screamPitch = 1f;

    private bool hasFallen = false;

    void Update()
    {
        if (hasFallen) return;

        if (transform.position.y < fallThresholdY)
        {
            hasFallen = true;
            PlayScream();
        }
    }

    private void PlayScream()
    {
        if (audioSource != null && screamClip != null)
        {
            audioSource.pitch = screamPitch;
            audioSource.PlayOneShot(screamClip);
        }
    }
}