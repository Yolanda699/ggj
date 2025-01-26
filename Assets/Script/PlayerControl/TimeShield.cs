using UnityEngine;

public class TimeShield : MonoBehaviour
{
    [Header("Audio Source for Shield Interaction")]
    public AudioSource shieldAudioSource; // AudioSource to play sound when the shield interacts

    // Triggered when another collider enters this object's trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag "Damage2"
        if (other.CompareTag("Damage2"))
        {
            // Log a message to the console
            Debug.Log("Damage2 object collided with the Time Shield, destroying the object!");

            // Play the sound if an AudioSource is assigned
            if (shieldAudioSource != null && !shieldAudioSource.isPlaying)
            {
                shieldAudioSource.Play();
            }

            // Destroy the collided object
            Destroy(other.gameObject);
        }
    }
}
