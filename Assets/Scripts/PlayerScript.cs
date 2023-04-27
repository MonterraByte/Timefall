using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
    public float playerSpeed = 1.0f;
    public float playerAcceleration = 1.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    private CharacterController characterController;
    private Animator animator;

    private Vector2 moveInput;
    private float jumpInput;
    private const float jumpInputMax = 0.1f;
    private bool meleeInput;

    private float coyoteTime;
    private const float coyoteTimeLength = 0.15f;
    private bool isGrounded;
    private bool isRight = true;

    private float attackTimer;
    private float currentAttackTimer;
    private int attackType = 0;
    private bool isAttacking = false;

    private void Start() {
        characterController = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();
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
        if (context.started && !this.isAttacking) {
            meleeInput = true;
        }
    }

    private void Update() {
        isGrounded = characterController.isGrounded;
        var velocity = characterController.velocity;

        if (!(this.attackType == 1)) {
            velocity.x = moveInput.x * playerSpeed;

            if (isRight && velocity.x < 0.0f)
            {
                this.transform.Rotate(0.0f, 180.0f, 0.0f);
                isRight = false;
            }

            if (!isRight && velocity.x > 0.0f)
            {
                this.transform.Rotate(0.0f, 180.0f, 0.0f);
                isRight = true;
            }
        }

        coyoteTime -= Time.deltaTime;
        if (isGrounded) {
            coyoteTime = coyoteTimeLength;

            if (velocity.y < 0.0f) {
                velocity.y = 0.0f;
            }
        }

        if (meleeInput && !this.isAttacking){
            if (!isGrounded) {
                this.animator.SetTrigger("AirAttack");
                this.attackTimer = 0.50f;
                this.attackType = 2;
            }
            else if (velocity.x != 0.0f) {
                this.animator.SetTrigger("NeutralAttack");
                this.attackType = 1;
                this.attackTimer = 1.0f;
            }
            else {
                this.animator.SetTrigger("NeutralAttack");
                this.attackTimer = 1.0f;
                this.attackType = 0;
            }
            meleeInput = false;
            this.currentAttackTimer = 0.0f;
            this.isAttacking = true;
        }

        if (jumpInput > 0.0f && CanJump()) {
            coyoteTime = 0.0f;
            velocity.y = Mathf.Sqrt(jumpHeight * 2.0f * -gravityValue);
            jumpInput = 0.0f;
        }
        jumpInput -= Time.deltaTime;

        velocity.x = Mathf.Lerp(velocity.x, moveInput.x * playerSpeed, playerAcceleration * Time.deltaTime);
        velocity.y += gravityValue * Time.deltaTime;

        if (this.currentAttackTimer < this.attackTimer) {
            switch (this.attackType) {
                case 1:
                    if (this.isRight) velocity.x = 10;
                    else velocity.x = -10;
                    this.transform.Rotate(0, 0, -45 * Time.deltaTime);

                    break;

                case 2:
                    this.transform.Rotate(0, 0, -720 * Time.deltaTime);

                    break;
            }

            this.currentAttackTimer += Time.deltaTime;
        }
        else {
            if (this.attackType != 0) {
                if (this.isRight) this.transform.rotation = new Quaternion(0, 0, 0, 0);
                else this.transform.rotation = new Quaternion(0, 180.0f, 0, 0);
            }

            this.attackType = 0;
            this.isAttacking = false;
        }

        characterController.Move(velocity * Time.deltaTime);
    }
}
