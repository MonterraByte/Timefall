using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public float maxHeight = 3.65f;

    private bool doorOpen = false;
    private bool doorClose = false;
    private float cooldown = 5.0f;
    private float startPoint;
    // Start is called before the first frame update
    void Start()
    {
        this.startPoint = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.doorOpen)
        {
            this.transform.position += Vector3.up * 3 * Time.deltaTime;
            if (this.transform.position.y >= (this.maxHeight + this.startPoint))
            {
                this.doorOpen = false;
                this.doorClose = true;
            }
        }

        if (this.doorClose)
        {
            if (this.cooldown == 0.0f)
            {
                this.transform.position -= Vector3.up * 3 * Time.deltaTime;
                if (this.transform.position.y <= this.startPoint)
                {
                    Vector3 pos = this.transform.position;
                    pos.y = this.startPoint;
                    this.transform.position = pos;
                    this.doorClose = false;
                }
            }
            else
            {
                this.cooldown -= Time.deltaTime;
                if (this.cooldown < 0.0f)
                {
                    this.cooldown = 0.0f;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!this.doorClose && !this.doorOpen)
        {
            this.doorOpen = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!this.doorClose && !this.doorOpen)
        {
            this.doorOpen = true;
        }
    }
}
