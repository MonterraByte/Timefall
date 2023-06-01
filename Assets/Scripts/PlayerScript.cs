using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
    public float playerSpeed = 1.0f;
    public float playerAcceleration = 1.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public int playerHealth = 100;

    public bool hasDoubleJump;

    private CharacterController characterController;

    private Vector2 moveInput;
    private float jumpInput;
    private const float jumpInputMax = 0.1f;

    private float coyoteTime;
    private const float coyoteTimeLength = 0.15f;
    private bool usedDoubleJump;
    private bool isGrounded;

    private void Start() {
        characterController = GetComponent<CharacterController>();
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
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        isGrounded = characterController.isGrounded || Physics.Raycast(transform.position, Vector3.down, (characterController.height / 2.0f) + 0.01f);
    }

    private void Update() {
        var velocity = characterController.velocity;

        var headingInput = new Vector3(0.0f, 0.0f, moveInput.x);
        if (headingInput.sqrMagnitude > 0.0025f) {
            gameObject.transform.forward = headingInput;
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
            }
            coyoteTime = 0.0f;
            velocity.y = Mathf.Sqrt(jumpHeight * 2.0f * -gravityValue);
            jumpInput = 0.0f;
        }
        jumpInput -= Time.deltaTime;

        velocity.x = Mathf.Lerp(velocity.x, moveInput.x * playerSpeed, playerAcceleration * Time.deltaTime);
        velocity.y += gravityValue * Time.deltaTime;
        velocity.z = 0.0f;

        characterController.Move(velocity * Time.deltaTime);
    }

    private void DamagePlayer(int damage) {
        TakeDamage(damage);
        VerifyPlayerHealth();
    }

    private void TakeDamage(int damage) {
        playerHealth -= damage;
    }

    private void VerifyPlayerHealth() {
        Debug.Log("Player health is now " + playerHealth);
        if (playerHealth <= 0) {
            Debug.Log("Player is dead");
            // TO DO: Game over
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Player collided with " + other.gameObject.name);
        if (other.gameObject.CompareTag("Bullet")) {
            DamagePlayer(other.gameObject.GetComponent<BulletController>().damage);
        }
    }

}
