using UnityEngine;

/// <summary>
/// Camera movement script for third person games.
/// This Script should not be applied to the camera! It is attached to an empty object and inside
/// it (as a child object) should be your game's MainCamera.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Tooltip("Enable to move the camera by holding the right mouse button. Does not work with joysticks.")]
    public bool clickToMoveCamera = false;
    [Tooltip("Enable zoom in/out when scrolling the mouse wheel. Does not work with joysticks.")]
    public bool canZoom = true;
    [Space]
    [Tooltip("The higher it is, the faster the camera moves. It is recommended to increase this value for games that uses joystick.")]
    public float sensitivity = 5f;

    [Tooltip("Camera Y rotation limits. The X axis is the maximum it can go up and the Y axis is the maximum it can go down.")]
    public Vector2 cameraLimit = new Vector2(-45, 40);
    [Tooltip("Offsite position for the camera to be on the player's right shoulder.")]
    public Vector3 rightShoulderOffset = new Vector3(0.5f, 1.5f, -2f);

    private float mouseX;
    private float mouseY;

    private Transform player;

    void Start()
    {
        // Find the player object with the "Player" tag
        player = GameObject.FindWithTag("Player").transform;

        // Lock and hide cursor if click-to-move is disabled
        if (!clickToMoveCamera)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        FollowPlayer();

        if (!IsCameraControlAllowed())
            return;

        HandleMouseRotation();
    }

    /// <summary>
    /// Makes the camera follow the player's position with an offset.
    /// </summary>
    void FollowPlayer()
    {
        // Calculate the camera offset relative to the player's position
        Vector3 shoulderOffset = transform.right * rightShoulderOffset.x +
                                 Vector3.up * rightShoulderOffset.y +
                                 rightShoulderOffset.z * transform.forward;
        transform.position = player.position + shoulderOffset;
    }

    /// <summary>
    /// Handles mouse rotation and clamps vertical angles based on camera limits.
    /// </summary>
    void HandleMouseRotation()
    {
        // Update mouse input for rotation
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY += Input.GetAxis("Mouse Y") * sensitivity;

        // Clamp the vertical rotation to prevent the camera from moving too far up or down
        mouseY = Mathf.Clamp(mouseY, cameraLimit.x, cameraLimit.y);

        // Apply rotation to the camera
        transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
    }

    /// <summary>
    /// Determines if camera control is allowed based on input settings.
    /// </summary>
    /// <returns>True if camera control is enabled; otherwise, false.</returns>
    bool IsCameraControlAllowed()
    {
        if (clickToMoveCamera && Input.GetAxisRaw("Fire2") == 0)
        {
            return false; // Right mouse button is required for control but not pressed
        }

        return true; // Camera control is allowed
    }
}
