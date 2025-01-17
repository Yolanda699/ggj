using System.Collections;
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

    public Vector3 respawnPosition;  // 存储出生点的位置
    private Renderer characterRenderer;  // 角色的渲染器
    private bool isDead = false;  // 角色是否死亡的状态

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // 获取角色的渲染器组件
        characterRenderer = GetComponent<Renderer>();

        // Message informing the user that they forgot to add an animator
        if (animator == null)
            Debug.LogWarning("Hey buddy, you don't have the Animator component in your player. Without it, the animations won't work.");

        respawnPosition = transform.position;
    }

    void Update()
    {
        if (isDead)
        {
            // 如果角色已经死亡，禁止所有输入
            return;
        }

        // Input checkers
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputJump = Input.GetAxis("Jump") == 1f;
        inputSprint = Input.GetAxis("Fire3") == 1f;
        inputCrouch = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton1);

        // Check if you pressed the crouch input key and change the player's state
        if (inputCrouch)
            isCrouching = !isCrouching;

        // Run and Crouch animation
        if (cc.isGrounded && animator != null)
        {
            // Crouch
            animator.SetBool("crouch", isCrouching);

            // Run
            float minimumSpeed = 0.9f;
            animator.SetBool("run", cc.velocity.magnitude > minimumSpeed);

            // Sprint
            isSprinting = cc.velocity.magnitude > minimumSpeed && inputSprint;
            animator.SetBool("sprint", isSprinting);
        }

        // Jump animation
        if (animator != null)
            animator.SetBool("air", cc.isGrounded == false);

        // Handle can jump or not
        if (inputJump && cc.isGrounded)
        {
            isJumping = true;
        }

        HeadHittingDetect();

        DetectDangerousCollision();
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            // 如果角色已经死亡，停止所有移动
            return;
        }

        // Sprinting velocity boost or crouching desacelerate
        float velocityAdittion = 0;
        if (isSprinting)
            velocityAdittion = sprintAdittion;
        if (isCrouching)
            velocityAdittion = -(velocity * 0.50f); // -50% velocity

        // Direction movement
        float directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
        float directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
        float directionY = 0;

        // Jump handler
        if (isJumping)
        {
            // Apply inertia and smoothness when climbing the jump
            directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.30f, jumpElapsedTime / jumpTime) * Time.deltaTime;

            // Jump timer
            jumpElapsedTime += Time.deltaTime;
            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                jumpElapsedTime = 0;
            }
        }

        // Add gravity to Y axis
        directionY = directionY - gravity * Time.deltaTime;

        // --- Character rotation ---
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

        // --- End rotation ---

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

        // 隐藏角色
        if (characterRenderer != null)
        {
            characterRenderer.enabled = false;  // 禁用渲染器，角色消失
        }

        // 禁用CharacterController，防止玩家操作
        if (cc != null)
        {
            cc.enabled = false;
        }

        // 禁用所有输入
        isDead = true;

        // 开始一个协程，5秒后复活
        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        // 等待5秒钟
        yield return new WaitForSeconds(5f);

        // 复活角色
        Respawn();
    }

    private void Respawn()
    {
        // 复活时恢复角色的位置
        transform.position = respawnPosition;

        // 恢复渲染器
        if (characterRenderer != null)
        {
            characterRenderer.enabled = true;  // 启用渲染器，角色重新显示
        }

        // 恢复CharacterController
        if (cc != null)
        {
            cc.enabled = true;
        }

        // 恢复输入控制
        isDead = false;

        Debug.Log("角色复活！");
    }
}
