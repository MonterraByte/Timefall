using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
    private const string ClimbLayer = "Climbable Wall";
    private const string PlayerLayer = "Player";

    public float playerSpeed = 1.0f;
    public float playerAcceleration = 1.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    public HealthUi healthUi;

    public float invincibilityDuration = 1.1f;
    private float damageCooldown;

    public int maxLives = 5;
    private int _lives;
    public int Lives {
        get => _lives;
        set {
            if (damageCooldown > 0.0f) {
                return;
            }
            damageCooldown = invincibilityDuration;

            _lives = Math.Clamp(value, 0, maxLives);
            if (healthUi != null) {
                healthUi.UpdateUi(Lives, Shields);
            }
        }
    }

    public int maxShields = 1;
    private int _shields;
    public int Shields {
        get => _shields;
        set {
            if (damageCooldown > 0.0f) {
                return;
            }
            damageCooldown = invincibilityDuration;

            _shields = Math.Clamp(value, 0, maxShields);
            if (healthUi != null) {
                healthUi.UpdateUi(Lives, Shields);
            }
        }
    }

    public Vector3 respawnLocation;
    private GameObject lastRespawnPointObject;

    public bool hasDoubleJump;
    public bool hasClimb;

    public Animator animator;
    private CharacterController characterController;
    private LayerMask climbLayerMask;
    private LayerMask playerLayerMask;
    private HookshotScript hookScript;

    private static readonly int jumpTrigger = Animator.StringToHash("Jump");
    private static readonly int doubleJumpTrigger = Animator.StringToHash("DoubleJump");
    private static readonly int hurtTrigger = Animator.StringToHash("Hurt");
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
        playerLayerMask = LayerMask.GetMask(PlayerLayer);
        hookScript = GetComponentsInChildren<HookshotScript>()[0];

        Lives = maxLives;
        Shields = 0;
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

    private void FixedUpdate() {
        isGrounded = characterController.isGrounded || Physics.Raycast(transform.position, Vector3.down, (characterController.height / 2.0f) + 0.01f, Physics.DefaultRaycastLayers ^ playerLayerMask);
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
        damageCooldown -= Time.deltaTime;

        velocity.x = Mathf.Lerp(velocity.x, moveInput.x * playerSpeed, playerAcceleration * Time.deltaTime);
        if (!this.onPlatform)
        {
            velocity.y += gravityValue * Time.deltaTime;
        }
        velocity.z = 0.0f;

        animator.SetFloat(runSpeedParameter, Mathf.Abs(velocity.x));
        characterController.Move(velocity * Time.deltaTime);
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
                GetComponentInChildren<Flamethrower>().enabled = true;
                break;

            case "Boomerang":
                GetComponentInChildren<Boomerang>().enabled = true;
                break;

            case "Moving":
                this.onPlatform = true;
                break;
            case "Bullet":
                TakeDamage(1);
                break;
        }
    }

    public void TakeDamage(int damage, bool environmental = false)
    {
        if (damageCooldown > 0.0f) {
            return;
        }

        if (Shields > 0) {
            Shields -= damage;
        } else {
            Lives -= damage;
        }

        animator.SetTrigger(hurtTrigger);

        if (environmental) {
            damageCooldown = Mathf.Infinity;
            StartCoroutine(QueuedMoveToRespawnPoint());
        }
    }

    public void MoveToRespawnPoint() {
        Debug.Log($"Moving player to {respawnLocation}");
        transform.position = respawnLocation;
    }

    private IEnumerator QueuedMoveToRespawnPoint() {
        yield return new WaitForSeconds(0.1f);
        MoveToRespawnPoint();
        damageCooldown = invincibilityDuration;
    }

    public void SetRespawnPoint(Vector3 point) {
        respawnLocation = point;
        if (lastRespawnPointObject != null) {
            lastRespawnPointObject.SetActive(true);
        }
        lastRespawnPointObject = null;
    }

    public void SetRespawnPoint(GameObject spawner) {
        respawnLocation = spawner.transform.position;
        if (lastRespawnPointObject != null) {
            lastRespawnPointObject.SetActive(true);
        }
        lastRespawnPointObject = spawner;
        lastRespawnPointObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Moving"))
        {
            this.onPlatform = false;
        }
    }
}
