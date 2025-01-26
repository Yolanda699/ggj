using UnityEngine;

public class Win : MonoBehaviour
{
    [Tooltip("The GameObject to deactivate when this object is set inactive.")]
    public GameObject targetToActivate; // Assign the GameObject in the Inspector

    void OnDisable()
    {
        // Check if the target object is assigned
        if (targetToActivate != null)
        {
            targetToActivate.SetActive(true);
        }
    }
}
