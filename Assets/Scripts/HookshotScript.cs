using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookshotScript : MonoBehaviour
{
    public float hookSpeed = 2.0f;

    private int hookState = 0;
    private bool isRight = true;
    
    private Animator animator;
    private BoxCollider boxCollider;
    private Camera mainCamera;
    private PlayerInput playerInput;
    private Vector3 vector;
    private CharacterController parentController;

    private float currentX;
    private float currentY;
    private float currentAngle;

    // Start is called before the first frame update
    void Start()
    {
        this.mainCamera = Camera.main;
        this.animator = GetComponentInParent<Animator>();
        this.boxCollider = GetComponent<BoxCollider>();
        this.playerInput = GetComponentInParent<PlayerInput>();
        this.parentController = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        updateSide();

        fireHook();

        moveHook();
    }

    void fireHook()
    {
        if (Mouse.current.rightButton.isPressed && this.hookState == 0)
        {
            this.boxCollider.enabled = true;
            this.animator.enabled = false;
            this.gameObject.layer = 3;
            this.hookState = 1;

            playerInput.actions["Move"].Disable();
            playerInput.actions["Melee"].Disable();
            playerInput.actions["Jump"].Disable();
            playerInput.actions["Counter"].Disable();

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 pos = this.mainCamera.ScreenToViewportPoint(mousePos);
            this.vector = pos - new Vector3(0.5f, 0.5f, 0);

            if (this.isRight) this.currentAngle = Mathf.Acos(Vector3.Dot(vector, Vector3.up) / vector.magnitude);
            else this.currentAngle = Mathf.Acos(Vector3.Dot(vector, Vector3.down) / vector.magnitude);

            this.currentX = (float)(Math.Sin(this.currentAngle));
            this.currentY = (float)(Math.Cos(this.currentAngle));

            if (!this.isRight)
            {
                this.currentX *= -1;
                this.currentY *= -1;
            }

            if (this.vector.x < 0)
            {
                this.transform.localPosition = new Vector3(-this.currentX, this.currentY, -0.5f);
                this.transform.Rotate(-20, 0, this.currentAngle * Mathf.Rad2Deg);
            }
            else
            {
                this.transform.localPosition = new Vector3(this.currentX, this.currentY, -0.5f);
                this.transform.Rotate(-20, 0, -this.currentAngle * Mathf.Rad2Deg);
            }
        }
    }

    void moveHook()
    {
        switch (this.hookState)
        {
            case 1:
                this.transform.localScale += new Vector3(0, this.hookSpeed, 0) * Time.deltaTime;

                if (this.vector.x < 0)
                {
                    Vector3 velocity = new Vector3(-this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                    this.transform.localPosition += velocity;
                    this.parentController.Move(velocity);
                }
                else
                {
                    this.transform.localPosition += new Vector3(this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                }

                if (this.transform.localScale.y > 10.0)
                {
                    this.hookState = 2;
                    this.boxCollider.enabled = false;
                }
                break;

            case 2:
                this.transform.localScale -= new Vector3(0, this.hookSpeed, 0) * Time.deltaTime;

                if (this.vector.x < 0) this.transform.localPosition -= new Vector3(-this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                else this.transform.localPosition -= new Vector3(this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;

                if (this.transform.localScale.y < 1.0)
                {
                    this.hookState = 0;
                    this.transform.localScale = new Vector3(0.1f, 1, 0.1f);
                    this.animator.enabled = true;
                    this.gameObject.layer = 1;
                    playerInput.actions["Move"].Enable();
                    playerInput.actions["Melee"].Enable();
                    playerInput.actions["Jump"].Enable();
                    playerInput.actions["Counter"].Enable();
                }
                break;

            case 3:
                this.transform.localScale -= new Vector3(0, this.hookSpeed, 0) * Time.deltaTime;

                if (this.vector.x < 0)
                {
                    Vector3 velocity = new Vector3(-this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                    this.transform.localPosition += velocity;
                    this.parentController.Move(velocity);
                }
                else
                {
                    Vector3 velocity = new Vector3(this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                    this.transform.localPosition += velocity;
                    this.parentController.Move(velocity);
                }

                if (this.transform.localScale.y < 1.0)
                {
                    this.hookState = 0;
                    this.transform.localScale = new Vector3(0.1f, 1, 0.1f);
                    this.animator.enabled = true;
                    this.gameObject.layer = 1;
                    playerInput.actions["Move"].Enable();
                    playerInput.actions["Melee"].Enable();
                    playerInput.actions["Jump"].Enable();
                    playerInput.actions["Counter"].Enable();
                }
                break;
        }
    }

    void updateSide()
    {
        float sideMove = this.playerInput.actions["Move"].ReadValue<Vector2>().x;

        if (sideMove < 0 && this.isRight) this.isRight = false;
        else if (sideMove > 0 && !this.isRight) this.isRight = true;
    }

    public Vector2 getCurrentCoordinates()
    {
        return new Vector2(this.currentX, this.currentY);
    }

    public bool getSide()
    {
        return this.isRight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hook" && this.hookState == 1)
        {
            this.hookState = 3;
        }
        if (this.hookState == 1)
        {
            this.hookState = 2;
            this.boxCollider.enabled = false;
        }
    }
}
