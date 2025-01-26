using UnityEngine;

public class Win : MonoBehaviour
{
    [Tooltip("The GameObject to activate when this object is set inactive.")]
    public GameObject targetToActivate; // Assign the GameObject in the Inspector

    [Header("Audio Settings")]
    public AudioSource winAudioSource; // AudioSource to play a sound when the player wins

    void OnDisable()
    {
        // Play win sound
        if (winAudioSource != null)
        {
            winAudioSource.Play();
        }

        // Activate the target GameObject
        if (targetToActivate != null)
        {
            targetToActivate.SetActive(true);
        }
    }
}
