using UnityEngine;
using UnityEngine.UI;

public class MakeGum : MonoBehaviour
{
    [Tooltip("The gum object to be activated.")]
    public GameObject gum; // Assign the gum GameObject in the Inspector

    [Tooltip("The hint text to show when close to the gum activation point.")]
    public GameObject hintText; // Assign the UI text GameObject in the Inspector

    [Tooltip("The distance within which the player can interact to make gum.")]
    public float interactionDistance = 3f;

    private Transform player; // Reference to the player's transform
    private bool isPlayerClose = false; // Tracks if the player is near the gum activation point
    private bool isGumActive = false; // Tracks if the gum is active

    void Start()
    {
        // Hide the hint text at the start
        if (hintText != null)
        {
            hintText.SetActive(false);
        }

        // Optionally, find the player by tag
        player = GameObject.FindWithTag("Player").transform;
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

            // Activate the gum when Q is pressed
            if (Input.GetKeyDown(KeyCode.Q) && gum != null)
            {
                isGumActive = !isGumActive; // Toggle the gum state
                gum.SetActive(isGumActive);
                Debug.Log("Gum is now " + (isGumActive ? "Active" : "Inactive"));
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
}
