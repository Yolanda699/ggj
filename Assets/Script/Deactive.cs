using UnityEngine;

public class Deactivate : MonoBehaviour
{
    [Tooltip("The GameObject to deactivate when this object is set inactive.")]
    public GameObject targetToDeactivate; // Assign the GameObject in the Inspector

    void OnDisable()
    {
        // Check if the target object is assigned
        if (targetToDeactivate != null)
        {
            // Set the target GameObject inactive
            targetToDeactivate.SetActive(false);
        }
    }
}
