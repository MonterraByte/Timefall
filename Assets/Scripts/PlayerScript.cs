using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
    public float playerSpeed = 1.0f;
    public float playerAcceleration = 1.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public float dashSpeed = 5.0f;

    private CharacterController characterController;
    private BoxCollider stickCollider;

    private Vector2 moveInput;
    private float jumpInput;
    private const float jumpInputMax = 0.1f;
    private bool meleeInput;

    private float coyoteTime;
    private const float coyoteTimeLength = 0.15f;
    private bool isGrounded;
    private bool isRight = true;
    private Vector3 playerVelocity;

    private void Start() {
        characterController = GetComponent<CharacterController>();

        this.stickCollider = this.transform.GetChild(1).gameObject.GetComponent<BoxCollider>();

        this.stickCollider.enabled = false;
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

    public void OnMelee(InputAction.CallbackContext context){
        if (context.started) {
            meleeInput = true;
            this.stickCollider.enabled = true;
        }
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

        if (meleeInput){
            if (!isGrounded) {
                //attack air
            }
            if (velocity.x != 0.0f) {
                velocity.x *= dashSpeed;
            }
            else {
                //normal punch
            }
            meleeInput = false;
            //this.stickCollider.enabled = false;
        }

        if (jumpInput > 0.0f && CanJump()) {
            coyoteTime = 0.0f;
            velocity.y = Mathf.Sqrt(jumpHeight * 2.0f * -gravityValue);
            jumpInput = 0.0f;
        }
        jumpInput -= Time.deltaTime;

        velocity.x = Mathf.Lerp(velocity.x, moveInput.x * playerSpeed, playerAcceleration * Time.deltaTime);
        velocity.y += gravityValue * Time.deltaTime;

        if (isRight && velocity.x < 0.0f) {
            this.transform.Rotate(0.0f, 180.0f, 0.0f);
            isRight = false;
        }

        if (!isRight && velocity.x > 0.0f) {
            this.transform.Rotate(0.0f, 180.0f, 0.0f);
            isRight = true;
        }

        characterController.Move(velocity * Time.deltaTime);
    }
}
