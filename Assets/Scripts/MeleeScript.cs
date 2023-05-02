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

    // Start is called before the first frame update

    public void OnMelee(InputAction.CallbackContext context)
    {
        if (context.started && !this.isAttacking)
        {
            StartAttack();
        }
    }

    void Start()
    {
        this.animator = GetComponent<Animator>();

        this.characterController = GetComponent<CharacterController>();

        this.playerInput = GetComponent<PlayerInput>();
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
                        this.transform.Rotate(0, 0, -45 * Time.deltaTime);
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

                if (!(this.attackType == 0))
                {
                    if (isRight) this.transform.rotation = new Quaternion(0, 0, 0, 0);
                    else this.transform.rotation = new Quaternion(0, 180.0f, 0, 0);
                }

                this.playerInput.actions["Move"].Enable();
            }
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    void StartAttack()
    {
        this.isAttacking = true;

        if (!characterController.isGrounded)
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
                    break;

                case < 0.0f:
                    this.attackType = 1;
                    this.isRight = false;
                    break;

                case > 0.0f:
                    this.attackType = 1;
                    this.isRight = true;
                    break;
            }

            this.animator.SetTrigger("NeutralAttack");
            this.currentAttackTime = 0.0f;
            this.attackTime = 1.0f;
            playerInput.actions["Move"].Disable();
        }
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
}
