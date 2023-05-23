using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool doorOpen = false;
    private bool doorClose = false;
    private float cooldown = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.doorOpen)
        {
            this.transform.position += Vector3.up * 3 * Time.deltaTime;
            if (this.transform.position.y >= 7.0f)
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
                if (this.transform.position.y <= 3.35f)
                {
                    Vector3 pos = this.transform.position;
                    pos.y = 3.35f;
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
