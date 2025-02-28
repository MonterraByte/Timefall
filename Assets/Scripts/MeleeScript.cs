using UnityEngine.InputSystem;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    public bool hasDash = false;
    public float dashSpeed = 10.0f;
    public float rotateDirection = -720.0f;

    public InputActionReference moveAction;
    public InputActionReference attackAction;
    public InputActionReference dashAction;

    private bool isAttacking = false;
    private int attackType = 0;

    private bool isRight;
    private bool dashRight;
    private float attackTime;
    private float currentAttackTime;
    private float addRotate = 0.0f;

    private Animator animator;
    private CharacterController characterController;
    private BoxCollider boxColliderStick;
    private GameObject forceField;

    private static readonly int meleeAttackTrigger = Animator.StringToHash("MeleeAttack");

    public void OnCounter(InputAction.CallbackContext context)
    {
        if (context.started && !this.isAttacking)
        {
            StartCounter();
        }
    }

    void Start()
    {
        this.animator = GetComponentInChildren<Animator>();

        this.characterController = GetComponent<CharacterController>();

        this.boxColliderStick = GameObject.Find("Wrench").GetComponent<BoxCollider>();

        this.boxColliderStick.enabled = false;

        this.forceField = transform.Find("ForceField").gameObject;

        attackAction.action.started += ctx => {
            if (ctx.started && !isAttacking) {
                StartAttack();
            }
        };

        dashAction.action.started += ctx =>
        {
            if (!isAttacking && ctx.started && this.hasDash)
            {
                StartDash();
            }
        };
    }

    private void OnEnable() {
        attackAction.action.Enable();
    }

    private void OnDisable() {
        attackAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSide();

        var velocity = characterController.velocity;

        if (this.isAttacking)
        {
            if (this.currentAttackTime < this.attackTime)
            {
                switch (this.attackType)
                {
                    case 1:
                        if (this.dashRight) velocity.x = this.dashSpeed;
                        else velocity.x = -this.dashSpeed;
                        this.transform.Rotate(90 * Time.deltaTime, 0, 0);
                        this.addRotate += 90 * Time.deltaTime;
                        characterController.Move(velocity * Time.deltaTime);
                        break;

                    case 2:
                        UpdateSide();
                        this.transform.Rotate(rotateDirection * Time.deltaTime, 0, 0);
                        break;
                }

                this.currentAttackTime += Time.deltaTime;
            }
            else
            {
                this.isAttacking = false;
                this.attackTime = 0;

                this.boxColliderStick.enabled = false;

                switch(this.attackType)
                {
                    case 1:
                        this.transform.Rotate(-this.addRotate, 0, 0);
                        break;

                    case 3:
                        this.forceField.SetActive(false);
                        break;
                }
                moveAction.action.Enable();
                this.attackType = 0;
            }
        }
    }

    void StartAttack()
    {
        this.isAttacking = true;

        this.boxColliderStick.enabled = true;

        this.attackType = 0;
        this.animator.SetTrigger(meleeAttackTrigger);

        this.currentAttackTime = 0.0f;
        this.attackTime = 0.5f;

        //moveAction.action.Disable();
    }
    

    void StartDash()
    {
        this.isAttacking = true;

        this.boxColliderStick.enabled = true;

        this.dashRight = this.isRight;

        this.attackType = 1;
        this.currentAttackTime = 0.0f;
        this.attackTime = 0.5f;
        this.addRotate = 0.0f;
        this.animator.SetTrigger(meleeAttackTrigger);
        moveAction.action.Disable();
    }

    void StartCounter()
    {
        this.forceField.SetActive(true);
        this.isAttacking = true;
        this.currentAttackTime = 0.0f;
        this.attackTime = 0.4f;
        this.attackType = 3;
        moveAction.action.Disable();
    }

    void UpdateSide()
    {
        float sideMove = moveAction.action.ReadValue<Vector2>().x;

        if (sideMove < 0 && this.isRight || sideMove > 0 && !this.isRight)
        {
            this.isRight = !this.isRight;
            this.rotateDirection *= -1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.attackType == 3)
        {
            this.gameObject.layer = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boots")
        {
            this.hasDash = true;
        }
    }
}
