using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
    private const string ClimbLayer = "Climbable Wall";

    public float playerSpeed = 1.0f;
    public float playerAcceleration = 1.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public int health = 100;

    public bool hasDoubleJump;
    public bool hasClimb;

    public Animator animator;
    private CharacterController characterController;
    private LayerMask climbLayerMask;
    private HookshotScript hookScript;

    private static readonly int jumpTrigger = Animator.StringToHash("Jump");
    private static readonly int doubleJumpTrigger = Animator.StringToHash("DoubleJump");
    private static readonly int runningParameter = Animator.StringToHash("Running");
    private static readonly int runSpeedParameter = Animator.StringToHash("RunSpeed");
    private static readonly int groundedParameter = Animator.StringToHash("Grounded");

    private Vector2 moveInput;
    private float jumpInput;
    private const float jumpInputMax = 0.1f;

    private float coyoteTime;
    private const float coyoteTimeLength = 0.15f;
    private bool usedDoubleJump;
    private bool isGrounded;

    private bool onPlatform = false;

    private void Start() {
        characterController = GetComponent<CharacterController>();
        climbLayerMask = LayerMask.GetMask(ClimbLayer);
        hookScript = GetComponentsInChildren<HookshotScript>()[0];
    }

    private bool CanJump() {
        return isGrounded || coyoteTime > 0.0f;
    }

    private bool CanDoubleJump() {
        return hasDoubleJump && !usedDoubleJump;
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (context.started) {
            jumpInput = jumpInputMax;
            hookScript.endSwing = true;
            this.onPlatform = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    public void setHealth(int newHealth)
    {
        this.health = newHealth;
    }

    private void FixedUpdate() {
        OnDeath();

        isGrounded = characterController.isGrounded || Physics.Raycast(transform.position, Vector3.down, (characterController.height / 2.0f) + 0.01f);
        animator.SetBool(groundedParameter, isGrounded);
    }

    private void Update()
    { 
        var velocity = characterController.velocity;

        var headingInput = new Vector3(moveInput.x, 0.0f, 0.0f);
        if (headingInput.sqrMagnitude > 0.0025f) {
            gameObject.transform.forward = headingInput;
            animator.SetBool(runningParameter, true);
        } else {
            animator.SetBool(runningParameter, false);
        }

        if (hasClimb && !isGrounded && Physics.Raycast(transform.position,
                new Vector3(Mathf.Sign(moveInput.x), 0.0f, 0.0f),
                characterController.radius + 0.1f, climbLayerMask)) {
            coyoteTime = coyoteTimeLength;
            usedDoubleJump = false;
        }

        coyoteTime -= Time.deltaTime;
        if (isGrounded) {
            coyoteTime = coyoteTimeLength;
            usedDoubleJump = false;

            if (velocity.y < 0.0f) {
                velocity.y = 0.0f;
            }
        }

        if (jumpInput > 0.0f && (CanJump() || CanDoubleJump())) {
            if (!CanJump()) {
                usedDoubleJump = true;
                animator.SetTrigger(doubleJumpTrigger);
            }
            coyoteTime = 0.0f;
            velocity.y = Mathf.Sqrt(jumpHeight * 2.0f * -gravityValue);
            jumpInput = 0.0f;
            animator.SetTrigger(jumpTrigger);
        }
        jumpInput -= Time.deltaTime;

        velocity.x = Mathf.Lerp(velocity.x, moveInput.x * playerSpeed, playerAcceleration * Time.deltaTime);
        if (!this.onPlatform)
        {
            velocity.y += gravityValue * Time.deltaTime;
        }
        velocity.z = 0.0f;

        animator.SetFloat(runSpeedParameter, Mathf.Abs(velocity.x));
        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnDeath()
    {
        if (this.health == 0)
        {
            RespawnManagerScript manager = GameObject.FindObjectOfType<RespawnManagerScript>();
            HealthManager healthManager = GameObject.FindObjectOfType<HealthManager>();

            if (manager != null)
            {
                manager.StartRespawn();
            }

            if (healthManager != null)
            {
                healthManager.takeHeart();
            }

            this.health = 100;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "Boots":
                this.hasDoubleJump = true;
                break;

            case "Claws":
                this.hasClimb = true;
                break;

            case "HookShot":
                GetComponentInChildren<HookshotScript>().enabled = true;
                break;

            case "Flame":
                GetComponentInChildren<Flamethrower>().setEnable();
                break;

            case "Boomerang":
                GetComponentInChildren<Boomerang>().setEnable();
                break;

            case "Moving":
                this.onPlatform = true;
                break;
            case "Bullet":
                Debug.Log("Bullet hit");
                this.health -= 10;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Moving")
        {
            this.onPlatform = false;
        }
    }
}
