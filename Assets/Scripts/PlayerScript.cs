using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
    public float playerSpeed = 1.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    private CharacterController characterController;

    private Vector2 moveInput;
    private float jumpInput;
    private const float jumpInputMax = 0.1f;

    private float coyoteTime;
    private const float coyoteTimeLength = 0.15f;
    private bool isGrounded;
    private Vector3 playerVelocity;

    private void Start() {
        characterController = GetComponent<CharacterController>();
    }

    private bool CanJump() {
        return isGrounded || coyoteTime > 0.0f;
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (context.started) {
            jumpInput = jumpInputMax;
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update() {
        isGrounded = characterController.isGrounded;
        var velocity = characterController.velocity;

        coyoteTime -= Time.deltaTime;
        if (isGrounded) {
            coyoteTime = coyoteTimeLength;

            if (velocity.y < 0.0f) {
                velocity.y = 0.0f;
            }
        }

        if (jumpInput > 0.0f && CanJump()) {
            coyoteTime = 0.0f;
            velocity.y = Mathf.Sqrt(jumpHeight * 2.0f * -gravityValue);
            jumpInput = 0.0f;
        }
        jumpInput -= Time.deltaTime;

        velocity.x = moveInput.x * playerSpeed;
        velocity.y += gravityValue * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}
