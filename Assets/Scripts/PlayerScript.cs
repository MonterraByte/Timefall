using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
    public float playerSpeed = 1.0f;
    public float playerAcceleration = 1.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    private CharacterController characterController;
    private BoxCollider stickCollider;
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
    private bool isDashing = false;
    private bool isDashingLeft = false;
    private bool isAttacking = false;
    private bool isAttackingAir = false;

    private void Start() {
        characterController = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();

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
        if (context.started && !this.isAttacking) {
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
                this.animator.SetTrigger("AirAttack");
                this.attackTimer = 0.50f;
                this.isAttackingAir = true;
            }
            else if (velocity.x != 0.0f) {
                this.animator.SetTrigger("NeutralAttack");
                this.isDashing = true;
                this.attackTimer = 0.99f;
                if (velocity.x < 0.0f) this.isDashingLeft = true;
                else this.isDashingLeft = false;
            }
            else {
                this.animator.SetTrigger("NeutralAttack");
                this.attackTimer = 0.99f;
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

        if (isRight && velocity.x < 0.0f) {
            this.transform.Rotate(0.0f, 180.0f, 0.0f);
            isRight = false;
        }

        if (!isRight && velocity.x > 0.0f) {
            this.transform.Rotate(0.0f, 180.0f, 0.0f);
            isRight = true;
        }

        if (this.isDashing && this.currentAttackTimer < this.attackTimer) {
            if (this.isDashingLeft) this.transform.position -= new Vector3(10 * Time.deltaTime, 0, 0);
            else this.transform.position += new Vector3(10 * Time.deltaTime, 0, 0);
            this.transform.Rotate(0, 0, -45 * Time.deltaTime);
            this.currentAttackTimer += Time.deltaTime;
        }
        else if (this.isDashing) {
            if (this.isDashingLeft) this.transform.rotation = new Quaternion(0, 180.0f, 0, 0);
            else this.transform.rotation = new Quaternion(0, 0, 0, 0);
            this.isDashing = false;
        }
        if (this.isAttackingAir && this.currentAttackTimer < this.attackTimer) {
            this.transform.Rotate(0, 0, -720 * Time.deltaTime);
            this.currentAttackTimer += Time.deltaTime;
        }
        else if (this.isAttackingAir) {
            if (this.isRight) this.transform.rotation = new Quaternion(0, 0, 0, 0);
            else this.transform.rotation = new Quaternion(0, 180.0f, 0, 0);
            this.isAttackingAir = false;
        }

        AnimatorStateInfo stateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
        if (this.isAttacking && stateInfo.IsName("Empty")) {
            this.stickCollider.enabled = false;
            this.isAttacking = false;
        }
        if (!this.isDashing)
            characterController.Move(velocity * Time.deltaTime);
    }
}
