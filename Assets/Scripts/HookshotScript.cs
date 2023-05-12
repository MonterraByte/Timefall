using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookshotScript : MonoBehaviour
{
    public float hookSpeed = 2.0f;

    private bool hookStart = false;
    private bool hookEnd = false;
    private bool isRight = true;
    
    private Animator animator;
    private BoxCollider boxCollider;
    private Camera mainCamera;
    private PlayerInput playerInput;
    private Vector3 vector;

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
        if (Mouse.current.rightButton.isPressed && !this.hookStart && !this.hookEnd)
        {
            this.boxCollider.enabled = true;
            this.animator.enabled = false;
            this.gameObject.layer = 3;
            this.hookStart = true;
            playerInput.actions["Move"].Disable();

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
        if (this.hookStart)
        {
            this.transform.localScale += new Vector3(0, this.hookSpeed, 0) * Time.deltaTime;

            if (this.vector.x < 0) this.transform.localPosition += new Vector3(-this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
            else this.transform.localPosition += new Vector3(this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;

            if (this.transform.localScale.y > 10.0)
            {
                this.hookStart = false;
                this.hookEnd = true;
                this.boxCollider.enabled = false;
            }
        }

        if (this.hookEnd)
        {
            this.transform.localScale -= new Vector3(0, this.hookSpeed, 0) * Time.deltaTime;

            if (this.vector.x < 0) this.transform.localPosition -= new Vector3(-this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
            else this.transform.localPosition -= new Vector3(this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;

            if (this.transform.localScale.y < 1.0)
            {
                this.hookEnd = false;
                this.transform.localScale = new Vector3(0.1f, 1, 0.1f);
                this.animator.enabled = true;
                playerInput.actions["Move"].Enable();
            }
        }
    }

    void updateSide()
    {
        float sideMove = this.playerInput.actions["Move"].ReadValue<Vector2>().x;

        if (sideMove < 0 && this.isRight) this.isRight = false;
        else if (sideMove > 0 && !this.isRight) this.isRight = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.hookStart)
        {
            this.hookEnd = true;
            this.hookStart = false;
            this.boxCollider.enabled = false;
        }
    }
}
