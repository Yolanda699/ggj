using UnityEngine;

public class MousePickupDestroy : MonoBehaviour
{
    public Camera mainCamera; // Main camera
    public float rayDistance = 100f; // Raycast distance
    public AudioClip chewingSound; // Audio for chewing
    private AudioSource audioSource; // Audio source for playing sounds
    public Material highlightMaterial; // Highlight material
    private Material originalMaterial; // Original material
    private Renderer currentRenderer; // Currently highlighted object

    void Start()
    {
        // Initialize the audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        HighlightObject();

        // Consume item with E
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryConsume();
        }
    }

    void TryConsume()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Pickup"))
            {
                // Play chewing sound
                if (chewingSound != null)
                {
                    audioSource.PlayOneShot(chewingSound);
                }

                // Destroy the item
                Destroy(hit.collider.gameObject);
            }
        }
    }

    void HighlightObject()
    {
        if (currentRenderer != null)
        {
            currentRenderer.material = originalMaterial; // Restore original material
            currentRenderer = null;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null && hit.collider.CompareTag("Pickup"))
            {
                originalMaterial = renderer.material; // Save original material
                renderer.material = highlightMaterial; // Apply highlight material
                currentRenderer = renderer;
            }
        }
    }
}
