using UnityEngine.InputSystem;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    public bool hasDash = false;
    public float dashSpeed = 10.0f;
    public float rotateDirection = -720.0f;

    public InputActionReference moveAction;
    public InputActionReference attackAction;

    private bool isAttacking = false;
    private int attackType = 0;

    private bool isRight;
    private float attackTime;
    private float currentAttackTime;

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

        this.boxColliderStick = GetComponentInChildren<BoxCollider>();

        this.boxColliderStick.enabled = false;

        this.forceField = transform.Find("ForceField").gameObject;

        attackAction.action.started += ctx => {
            if (ctx.started && !isAttacking) {
                StartAttack();
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
        var velocity = characterController.velocity;

        if (this.isAttacking)
        {
            if (this.currentAttackTime < this.attackTime)
            {
                switch (this.attackType)
                {
                    case 1:
                        if (this.isRight) velocity.x = this.dashSpeed;
                        else velocity.x = -this.dashSpeed;
                        this.transform.Rotate(90 * Time.deltaTime, 0, 0);
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
                    case 2:
                        if (isRight) this.transform.rotation = new Quaternion(0, 0, 0, 0);
                        else this.transform.rotation = new Quaternion(0, 180.0f, 0, 0);
                        break;

                    case 3:
                        //this.gameObject.layer = 0;
                        this.forceField.SetActive(false);
                        moveAction.action.Enable();
                        break;
                }

                this.attackType = 0;
            }
        }
    }

    void StartAttack()
    {
        this.isAttacking = true;

        this.boxColliderStick.enabled = true;

        var velocity = characterController.velocity;

        if (velocity.y != 0)
        {
            this.attackType = 2;
            this.animator.SetTrigger(meleeAttackTrigger);
            this.currentAttackTime = 0.0f;
            this.attackTime = 0.5f;
            this.rotateDirection = 720.0f;

            float sideMove = moveAction.action.ReadValue<Vector2>().x;

            if (sideMove < 0) this.isRight = false;
            else this.isRight = true;
        }
        if (!this.hasDash)
        {
            this.attackType = 0;
            this.animator.SetTrigger(meleeAttackTrigger);

            this.currentAttackTime = 0.0f;
            this.attackTime = 0.5f;
        }
        else
        {
            float sideMove = moveAction.action.ReadValue<Vector2>().x;

            switch (sideMove)
            {
                case 0.0f:
                    this.attackType = 0;
                    this.animator.SetTrigger(meleeAttackTrigger);
                    break;

                case < 0.0f:
                    this.attackType = 1;
                    this.isRight = false;
                    this.animator.SetTrigger(meleeAttackTrigger);
                    break;

                case > 0.0f:
                    this.attackType = 1;
                    this.isRight = true;
                    this.animator.SetTrigger(meleeAttackTrigger);
                    break;
            }

            this.currentAttackTime = 0.0f;
            this.attackTime = 0.5f;
        }
    }

    void StartCounter()
    {
        //this.animator.SetTrigger("Counter");
        this.forceField.SetActive(true);
        this.isAttacking = true;
        this.currentAttackTime = 0.0f;
        this.attackTime = 0.4f;
        this.attackType = 3;
        //this.gameObject.layer = 2;
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
