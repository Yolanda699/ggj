﻿using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Tooltip("Speed ​​​at which the character moves. It is not affected by gravity or jumping.")]
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
    bool jumpState = false;

    // Inputs
    float inputHorizontal;
    float inputVertical;
    bool inputJump;
    bool inputCrouch;
    bool inputSprint;

    Animator animator;
    CharacterController cc;
    private ShieldManager shieldManager; // 护盾管理

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        shieldManager = GetComponent<ShieldManager>();

        if (animator == null)
            Debug.LogWarning("没有找到 Animator 组件，动画将无法正常运行。");

        if (shieldManager == null)
            Debug.LogWarning("没有找到 ShieldManager 脚本，护盾功能将不可用。");
    }

    void Update()
    {
        // Input checkers
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputJump = Input.GetAxis("Jump") == 1f;
        inputSprint = Input.GetAxis("Fire3") == 1f;
        inputCrouch = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton1);

        // Toggle crouch state
        if (inputCrouch)
            isCrouching = !isCrouching;

        // Animation handling
        HandleAnimations();

        // Jump handling
        HandleJump();

        HeadHittingDetect();

        DetectDangerousCollision();
    }

    private void FixedUpdate()
    {
        // Movement calculations
        HandleMovement();
    }

    private void HandleAnimations()
    {
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
    }

    private void HandleMovement()
    {
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

        Vector3 moviment = (Vector3.up * directionY) + (forward + right);
        cc.Move(moviment);
    }

    // 将跳跃功能提取为一个函数
    private void HandleJump()
    {
        if (inputJump && cc.isGrounded)
        {
            if(jumpState == true)
            {
                isJumping = true;
            }   
        }
    }

    public void ActivateJump()
    {
        jumpState = true;
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
                GetComponent<DeathHandle>()?.HandleDeath();
                break;
            }

            if (hit.CompareTag("Damage1"))
            {
                GetComponent<DeathHandle>()?.HandleDeath();
                break;
            }

            if (hit.CompareTag("Damage2"))
            {
                GetComponent<DeathHandle>()?.HandleDeath();
                break;
            }
        }
    }
}
