using UnityEngine;

public class DeactivateOnDestroy : MonoBehaviour
{
    public GameObject targetToDeactivate; // Assign the GameObject to deactivate in the Inspector

    void OnDestroy()
    {
        if (targetToDeactivate != null)
        {
            targetToDeactivate.SetActive(false);
        }
    }
}
