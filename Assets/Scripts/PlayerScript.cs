using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
    private const float coyoteTimeLength = 0.15f;
    public float playerSpeed = 1.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    private CharacterController characterController;

    private Vector2 moveInput;
    private bool jumpInput;

    private float coyoteTime;
    private bool isGrounded;
    private Vector3 playerVelocity;

    private void Start() {
        characterController = GetComponent<CharacterController>();
    }

    private bool CanJump() {
        return isGrounded || coyoteTime > 0.0f;
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (context.started && CanJump()) {
            jumpInput = true;
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update() {
        isGrounded = characterController.isGrounded;
        var velocity = characterController.velocity;

        velocity.x = moveInput.x * playerSpeed;
        velocity.y += gravityValue * Time.deltaTime;

        if (isGrounded) {
            coyoteTime = coyoteTimeLength;

            if (velocity.y < 0.0f) {
                velocity.y = 0.0f;
            }
        }

        velocity.x = moveInput.x * playerSpeed;
        velocity.y += gravityValue * Time.deltaTime;

        coyoteTime -= Time.deltaTime;
        if (jumpInput) {
            coyoteTime = 0.0f;
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            jumpInput = false;
        }

        characterController.Move(velocity * Time.deltaTime);
    }
}
