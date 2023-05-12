using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookshotScript : MonoBehaviour
{
    private bool hookStart = false;
    private float currentAngle;
    private float currentTime;
    private Animator animator;
    private Camera mainCamera;
    private Vector3 vector;

    // Start is called before the first frame update
    void Start()
    {
        this.mainCamera = Camera.main;
        this.animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        fireHook();

        if (this.hookStart)
        {
            if (this.currentTime < 3.0f)
            {
                this.currentTime += Time.deltaTime;
                this.transform.localScale += new Vector3(0, 0.5f, 0) * Time.deltaTime;

                float x = (float)(Math.Sin(this.currentAngle));
                float y = (float)(Math.Cos(this.currentAngle));

                if (this.vector.x < 0) this.transform.localPosition += new Vector3(-x, y, 0) / 2 * Time.deltaTime;
                else this.transform.localPosition += new Vector3(x, y, 0) / 2 * Time.deltaTime;
            }
            else
            {
                this.currentTime = 0.0f;
                this.hookStart = false;
                this.transform.localScale = new Vector3(0.1f, 1, 0.1f);
                this.animator.enabled = true;
            }
        }
    }

    void fireHook()
    {
        if (Mouse.current.rightButton.isPressed && !this.hookStart)
        {
            this.animator.enabled = false;
            this.gameObject.layer = 3;
            this.hookStart = true;
            this.currentTime = 0.0f;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 pos = this.mainCamera.ScreenToViewportPoint(mousePos);
            this.vector = pos - new Vector3(0.5f, 0.5f, 0);

            this.currentAngle = Mathf.Acos(Vector3.Dot(vector, Vector3.up) / vector.magnitude);

            float x = (float)(Math.Sin(this.currentAngle));
            float y = (float)(Math.Cos(this.currentAngle));


            if (this.vector.x < 0)
            {
                this.transform.localPosition = new Vector3(-x, y, -0.5f);
                this.transform.Rotate(-20, 0, this.currentAngle * Mathf.Rad2Deg);
            }
            else
            {
                this.transform.localPosition = new Vector3(x, y, -0.5f);
                this.transform.Rotate(-20, 0, -this.currentAngle * Mathf.Rad2Deg);
            }
        }
    }
}
