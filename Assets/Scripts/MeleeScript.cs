using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    private bool isAttacking = false;
    private int attackType = 0;

    private bool isRight;
    private float attackTime;
    private float currentAttackTime;
    private float rotateDirection = -720.0f;

    private Animator animator;
    private CharacterController characterController;
    private PlayerInput playerInput;
    private BoxCollider boxColliderStick;

    // Start is called before the first frame update

    public void OnMelee(InputAction.CallbackContext context)
    {
        if (context.started && !this.isAttacking)
        {
            StartAttack();
        }
    }

    public void OnCounter(InputAction.CallbackContext context)
    {
        if (context.started && !this.isAttacking)
        {
            StartCounter();
        }
    }

    void Start()
    {
        this.animator = GetComponent<Animator>();

        this.characterController = GetComponent<CharacterController>();

        this.playerInput = GetComponent<PlayerInput>();

        this.boxColliderStick = GetComponentInChildren<BoxCollider>();

        this.boxColliderStick.enabled = false;
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
                        if (this.isRight) velocity.x = 10;
                        else velocity.x = -10;
                        this.transform.Rotate(0, 0, -90 * Time.deltaTime);
                        break;

                    case 2:
                        UpdateSide();
                        this.transform.Rotate(0, 0, this.rotateDirection * Time.deltaTime);
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
                        this.gameObject.layer = 0;
                        break;
                }

                this.attackType = 0;
                this.playerInput.actions["Move"].Enable();
            }

            characterController.Move(velocity * Time.deltaTime);
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
            this.animator.SetTrigger("AirAttack");
            this.currentAttackTime = 0.0f;
            this.attackTime = 0.5f;
            this.rotateDirection = -720.0f;

            float sideMove = this.playerInput.actions["Move"].ReadValue<Vector2>().x;

            if (sideMove < 0) this.isRight = false;
            else this.isRight = true;
        }

        else
        {
            float sideMove = this.playerInput.actions["Move"].ReadValue<Vector2>().x;

            switch (sideMove)
            {
                case 0.0f:
                    this.attackType = 0;
                    this.animator.SetTrigger("NeutralAttack");
                    break;

                case < 0.0f:
                    this.attackType = 1;
                    this.isRight = false;
                    this.animator.SetTrigger("DashAttack");
                    break;

                case > 0.0f:
                    this.attackType = 1;
                    this.isRight = true;
                    this.animator.SetTrigger("DashAttack");
                    break;
            }

            this.currentAttackTime = 0.0f;
            this.attackTime = 0.5f;
            playerInput.actions["Move"].Disable();
        }
    }

    void StartCounter()
    {
        this.animator.SetTrigger("Counter");
        this.isAttacking = true;
        this.currentAttackTime = 0.0f;
        this.attackTime = 0.2f;
        this.attackType = 3;
        this.gameObject.layer = 2;
        playerInput.actions["Move"].Disable();
    }

    void UpdateSide()
    {
        float sideMove = this.playerInput.actions["Move"].ReadValue<Vector2>().x;

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
}
