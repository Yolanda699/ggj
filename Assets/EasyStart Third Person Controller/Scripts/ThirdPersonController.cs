using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Tooltip("Speed ​​at which the character moves. It is not affected by gravity or jumping.")]
    public float velocity = 5f;
    [Tooltip("This value is added to the speed value while the character is sprinting.")]
    public float sprintAdittion = 3.5f;
    [Tooltip("The higher the value, the higher the character will jump.")]
    public float jumpForce = 18f;
    [Tooltip("Stay in the air. The higher the value, the longer the character floats before falling.")]
    public float jumpTime = 0.85f;
    [Space]
    [Tooltip("Force that pulls the player down. Changing this value causes all movement, jumping and falling to be changed as well.")]
    public float gravity = 9.8f;

    float jumpElapsedTime = 0;

    // Player states
    bool isJumping = false;
    bool isSprinting = false;
    bool isCrouching = false;

    // Inputs
    float inputHorizontal;
    float inputVertical;
    bool inputJump;
    bool inputCrouch;
    bool inputSprint;

    Animator animator;
    CharacterController cc;

    [Tooltip("Objects to reactivate during respawn.")]
    public List<GameObject> objectsToReactivate;

    public Vector3 respawnPosition;
    private Renderer characterRenderer;
    private bool isDead = false;
    public GameObject deathPopupUI;

    [Tooltip("AudioSource for footsteps sound.")]
    public AudioSource footstepsAudioSource; // 脚步声

    [Tooltip("AudioSource for jump sound.")]
    public AudioSource jumpAudioSource; // 跳跃声

    [Tooltip("AudioSource for death sound.")]
    public AudioSource deathAudioSource; // 死亡音效

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterRenderer = GetComponent<Renderer>();

        if (animator == null)
            Debug.LogWarning("Hey buddy, you don't have the Animator component in your player. Without it, the animations won't work.");

        respawnPosition = transform.position;

        if (deathPopupUI != null)
        {
            deathPopupUI.SetActive(false);
        }

        if (footstepsAudioSource == null)
        {
            Debug.LogWarning("No AudioSource assigned for footsteps sound.");
        }
        else
        {
            footstepsAudioSource.loop = true; // 确保音效循环播放
        }

        if (jumpAudioSource == null)
        {
            Debug.LogWarning("No AudioSource assigned for jump sound.");
        }

        if (deathAudioSource == null)
        {
            Debug.LogWarning("No AudioSource assigned for death sound.");
        }
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputJump = Input.GetAxis("Jump") == 1f;
        inputSprint = Input.GetAxis("Fire3") == 1f;
        inputCrouch = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton1);

        if (inputCrouch)
            isCrouching = !isCrouching;

        if (cc.isGrounded && animator != null)
        {
            animator.SetBool("crouch", isCrouching);

            float minimumSpeed = 0.9f;
            animator.SetBool("run", cc.velocity.magnitude > minimumSpeed);

            isSprinting = cc.velocity.magnitude > minimumSpeed && inputSprint;
            animator.SetBool("sprint", isSprinting);
        }

        if (animator != null)
            animator.SetBool("air", cc.isGrounded == false);

        if (inputJump && cc.isGrounded)
        {
            isJumping = true;

            // 播放跳跃声音
            if (jumpAudioSource != null)
            {
                jumpAudioSource.Play();
            }
        }

        HandleFootstepAudio(); // 检测并处理脚步声

        HeadHittingDetect();
        DetectDangerousCollision();
    }

    private void HandleFootstepAudio()
    {
        // 判断玩家是否正在移动
        bool isMoving = (inputHorizontal != 0 || inputVertical != 0) && cc.isGrounded;

        if (isMoving && !footstepsAudioSource.isPlaying)
        {
            footstepsAudioSource.Play(); // 播放脚步声
        }
        else if (!isMoving && footstepsAudioSource.isPlaying)
        {
            footstepsAudioSource.Stop(); // 停止脚步声
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        float velocityAdittion = 0;
        if (isSprinting)
            velocityAdittion = sprintAdittion;
        if (isCrouching)
            velocityAdittion = -(velocity * 0.50f);

        float directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
        float directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
        float directionY = 0;

        if (isJumping)
        {
            directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.30f, jumpElapsedTime / jumpTime) * Time.deltaTime;

            jumpElapsedTime += Time.deltaTime;
            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                jumpElapsedTime = 0;
            }
        }

        directionY = directionY - gravity * Time.deltaTime;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        forward = forward * directionZ;
        right = right * directionX;

        if (directionX != 0 || directionZ != 0)
        {
            float angle = Mathf.Atan2(forward.x + right.x, forward.z + right.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        Vector3 verticalDirection = Vector3.up * directionY;
        Vector3 horizontalDirection = forward + right;

        Vector3 moviment = verticalDirection + horizontalDirection;
        cc.Move(moviment);
    }

    void HeadHittingDetect()
    {
        float headHitDistance = 1.1f;
        Vector3 ccCenter = transform.TransformPoint(cc.center);
        float hitCalc = cc.height / 2f * headHitDistance;

        if (Physics.Raycast(ccCenter, Vector3.up, hitCalc))
        {
            jumpElapsedTime = 0;
            isJumping = false;
        }
    }

    private void DetectDangerousCollision()
    {
        Vector3 bottom = transform.position + cc.center - Vector3.up * (cc.height / 2 - cc.radius);
        Vector3 top = transform.position + cc.center + Vector3.up * (cc.height / 2 - cc.radius);
        float radius = cc.radius;

        Collider[] hits = Physics.OverlapCapsule(bottom, top, radius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Danger"))
            {
                HandleDeath();
                break;
            }
        }
    }

    private void HandleDeath()
    {
        Debug.Log("角色死亡！");

        // 播放死亡音效
        if (deathAudioSource != null)
        {
            deathAudioSource.Play();
        }

        if (deathPopupUI != null)
        {
            deathPopupUI.SetActive(true);
        }

        if (characterRenderer != null)
        {
            characterRenderer.enabled = false;
        }

        if (cc != null)
        {
            cc.enabled = false;
        }

        isDead = true;

        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        if (deathPopupUI != null)
        {
            deathPopupUI.SetActive(false);
        }

        foreach (GameObject obj in objectsToReactivate)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        Respawn();
    }

    private void Respawn()
    {
        transform.position = respawnPosition;

        if (characterRenderer != null)
        {
            characterRenderer.enabled = true;
        }

        if (cc != null)
        {
            cc.enabled = true;
        }

        isDead = false;

        Debug.Log("角色复活！");

        foreach (GameObject pickup in GameObject.FindGameObjectsWithTag("Pickup"))
        {
            pickup.SetActive(true);
        }
    }
}
