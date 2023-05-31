using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookshotScript : MonoBehaviour
{
    public float hookSpeed = 2.0f;
    public bool endSwing = false;

    private int hookState = 0;
    private bool isRight = true;
    private bool hook = false;
    
    private Animator animator;
    private BoxCollider boxCollider;
    private Camera mainCamera;
    private PlayerInput playerInput;
    private Vector3 vector;
    private CharacterController parentController;
    private Transform parentTransform;
    private Rigidbody parentRB;
    private LineRenderer lineRenderer;
    private PlayerScript playerScript;

    private float currentX;
    private float currentY;
    private float currentAngle;
    private Vector3 swingPoint;
    private int sideSwing = 1;

    // Start is called before the first frame update
    void Start()
    {
        this.mainCamera = Camera.main;
        this.animator = GetComponentInParent<Animator>();
        this.boxCollider = GetComponent<BoxCollider>();
        this.playerInput = GetComponentInParent<PlayerInput>();
        this.parentController = GetComponentInParent<CharacterController>();
        this.parentTransform = GetComponentInParent<Transform>();
        this.lineRenderer = GetComponentInParent<LineRenderer>();
        this.playerScript = GetComponentInParent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        updateSide();

        fireHook();

        moveHook();
    }

    public void OnHook(InputAction.CallbackContext context)
    {
        if (context.started && this.hookState == 0 && !this.hook)
        {
            this.hook = true;
        }
    }

    void fireHook()
    {
        if (this.hook)
        {
            this.hook = false;
            this.boxCollider.enabled = true;
            this.animator.enabled = false;
            this.gameObject.layer = 3;
            this.hookState = 1;
            this.endSwing = false;

            playerInput.actions["Move"].Disable();
            playerInput.actions["Melee"].Disable();
            playerInput.actions["Jump"].Disable();
            playerInput.actions["Counter"].Disable();
        
            Plane playerplane = new Plane(new Vector3(0, 0, 1), transform.position);
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            float hitdist;

            if (playerplane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                if (this.isRight) this.vector = targetPoint - this.parentTransform.position - Vector3.right / 2;
                else this.vector = targetPoint - this.parentTransform.position + Vector3.right / 2;
            }

            if (this.isRight) this.currentAngle = Mathf.Acos(Vector3.Dot(this.vector, Vector3.up) / this.vector.magnitude);
            else this.currentAngle = Mathf.Acos(Vector3.Dot(this.vector, Vector3.down) / this.vector.magnitude);

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

                if (this.vector.x < 0) this.transform.localPosition += new Vector3(-this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                else this.transform.localPosition += new Vector3(this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;

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
                
                if (this.isRight && this.vector.x < 0)
                {
                    Vector3 velocity = new Vector3(-this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                    this.transform.localPosition -= velocity;
                    this.parentController.Move(velocity);
                }
                else if (this.isRight && this.vector.x >= 0)
                {
                    Vector3 velocity = new Vector3(this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                    this.transform.localPosition -= velocity;
                    this.parentController.Move(velocity);
                }
                else if (!this.isRight && this.vector.x < 0)
                {
                    this.transform.localPosition -= new Vector3(-this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                    this.parentController.Move(new Vector3(this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime);
                }
                else
                {
                    this.transform.localPosition -= new Vector3(this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                    this.parentController.Move(new Vector3(-this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime);
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

            case 4:
                this.transform.localScale -= new Vector3(0, this.hookSpeed, 0) * Time.deltaTime;

                if (this.isRight && this.vector.x < 0)
                {
                    Vector3 velocity = new Vector3(-this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                    this.transform.localPosition -= velocity;
                    this.parentController.Move(velocity);
                }
                else if (this.isRight && this.vector.x >= 0)
                {
                    Vector3 velocity = new Vector3(this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                    this.transform.localPosition -= velocity;
                    this.parentController.Move(velocity);
                }
                else if (!this.isRight && this.vector.x < 0)
                {
                    this.transform.localPosition -= new Vector3(-this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                    this.parentController.Move(new Vector3(this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime);
                }
                else
                {
                    this.transform.localPosition -= new Vector3(this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime;
                    this.parentController.Move(new Vector3(-this.currentX, this.currentY, 0) * this.hookSpeed * Time.deltaTime);
                }

                if (this.transform.localScale.y < 1.0)
                {
                    this.transform.localScale = new Vector3(0.1f, 1.0f, 0.1f);
                    if (this.parentTransform.position.x < this.swingPoint.x) {
                        this.parentTransform.position = this.swingPoint + new Vector3((float)-Math.Sqrt(2), (float)-Math.Sqrt(2), 0);
                    }
                    else
                    {
                        this.parentTransform.position = this.swingPoint + new Vector3((float)Math.Sqrt(2), (float)-Math.Sqrt(2), 0);
                    }
                    this.lineRenderer.enabled = true;
                    this.transform.localScale = Vector3.zero;
                    this.hookState = 5;
                    playerInput.actions["Jump"].Enable();
                }
                break;

            case 5:
                swingMotion();
                break;
        }

    }

    void updateSide()
    {
        float sideMove = this.playerInput.actions["Move"].ReadValue<Vector2>().x;

        if (sideMove < 0 && this.isRight) this.isRight = false;
        else if (sideMove > 0 && !this.isRight) this.isRight = true;
    }

    void swingMotion()
    {
        if (this.endSwing)
        {
            this.hookState = 0;
            this.transform.localScale = new Vector3(0.1f, 1, 0.1f);
            this.animator.enabled = true;
            this.gameObject.layer = 1;
            this.lineRenderer.enabled = false;
            this.endSwing = false;
            playerInput.actions["Melee"].Enable();
            playerInput.actions["Counter"].Enable();
            playerInput.actions["Move"].Enable();
        }

        Vector3 velocity = this.swingPoint - this.parentTransform.position;
        Vector3 movement = new Vector3(-velocity.y, velocity.x, 0.0f);

        if (this.swingPoint.y < this.parentTransform.position.y)
        {
            this.sideSwing *= -1;
        }

        Vector3 swingMovement = movement.normalized * this.sideSwing * Time.deltaTime;

        this.parentController.Move(swingMovement);
        this.lineRenderer.SetPosition(0, this.parentTransform.position);
        this.lineRenderer.SetPosition(1, this.swingPoint);
    }

    public Vector2 getCurrentCoordinates()
    {
        return new Vector2(this.currentX, this.currentY);
    }

    public bool getSide()
    {
        return !(this.isRight ^ (this.vector.x < 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hook" && this.hookState == 1)
        {
            this.hookState = 3;
        }
        if (other.gameObject.tag == "Swing" && this.hookState == 1)
        {
            this.hookState = 4;
            this.swingPoint = other.transform.position;
        }
        if (this.hookState == 1)
        {
            this.hookState = 2;
            this.boxCollider.enabled = false;
        }
    }
}
