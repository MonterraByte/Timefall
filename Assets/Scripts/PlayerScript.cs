using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
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

    public void OnJump(InputAction.CallbackContext context) {
        if (context.started && isGrounded) {
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
            coyoteTime = 2.0f;

            if (velocity.y < 0.0f) {
                velocity.y = 0.0f;
            }
        }

        velocity.x = moveInput.x * playerSpeed;
        velocity.y += gravityValue * Time.deltaTime;

        coyoteTime -= Time.deltaTime;
        if (jumpInput && coyoteTime > 0.0f) {
            coyoteTime = 0.0f;
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            jumpInput = false;
        }

        characterController.Move(velocity * Time.deltaTime);
    }
}
