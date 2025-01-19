using UnityEngine;
using System.Collections;

public class MakeGum : MonoBehaviour
{
    [Tooltip("The gum object to be activated.")]
    public GameObject gum; // Assign the gum GameObject in the Inspector

    [Tooltip("The hint text to show when close to the gum activation point.")]
    public GameObject hintText; // Assign the UI text GameObject in the Inspector

    [Tooltip("The distance within which the player can interact to make gum.")]
    public float interactionDistance = 3f;

    [Tooltip("The AudioSource to play when Q is pressed.")]
    public AudioSource gumAudioSource; // Assign an AudioSource in the Inspector

    private Transform player; // Reference to the player's transform
    private bool isPlayerClose = false; // Tracks if the player is near the gum activation point
    private bool isGumActive = false; // Tracks if the gum is active
    private bool isAudioPlaying = false; // Tracks if the audio is currently playing

    void Start()
    {
        // Hide the hint text at the start
        if (hintText != null)
        {
            hintText.SetActive(false);
        }

        // Optionally, find the player by tag
        player = GameObject.FindWithTag("Player").transform;

        // Ensure the AudioSource is set
        if (gumAudioSource == null)
        {
            Debug.LogError("No AudioSource assigned for gum activation!");
        }
    }

    void Update()
    {
        // Check the distance between the player and the gum activation point
        if (Vector3.Distance(player.position, transform.position) <= interactionDistance)
        {
            if (!isPlayerClose)
            {
                ShowHint();
                isPlayerClose = true;
            }

            // Activate the gum after audio when Q is pressed
            if (Input.GetKeyDown(KeyCode.Q) && gum != null && !isAudioPlaying)
            {
                StartCoroutine(ActivateGumAfterAudio());
            }
        }
        else
        {
            if (isPlayerClose)
            {
                HideHint();
                isPlayerClose = false;
            }
        }
    }

    void ShowHint()
    {
        if (hintText != null)
        {
            hintText.SetActive(true);
        }
    }

    void HideHint()
    {
        if (hintText != null)
        {
            hintText.SetActive(false);
        }
    }

    IEnumerator ActivateGumAfterAudio()
    {
        isAudioPlaying = true;

        // Play the audio
        if (gumAudioSource != null)
        {
            gumAudioSource.Play();
            yield return new WaitForSeconds(gumAudioSource.clip.length); // Wait for the audio to finish
        }
        else
        {
            Debug.LogWarning("Gum AudioSource is missing or not set!");
        }

        // Activate or deactivate the gum
        isGumActive = !isGumActive;
        gum.SetActive(isGumActive);
        Debug.Log("Gum is now " + (isGumActive ? "Active" : "Inactive"));

        // Set the hint text inactive instead of destroying it
        if (hintText != null)
        {
            hintText.SetActive(false);
        }

        isAudioPlaying = false;
    }
}
