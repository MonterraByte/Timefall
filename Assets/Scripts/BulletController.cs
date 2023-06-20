using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float force = 5f;

    private Rigidbody rb;
    private int projectileLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        projectileLayer = LayerMask.NameToLayer("Player Projectile");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }   

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag + " has entered");

        if (other.gameObject.CompareTag("ForceField"))
        {
            Debug.Log("im here");
            this.gameObject.layer = projectileLayer;
            Vector3 velocity = this.rb.velocity;
            rb.velocity = -velocity * this.force;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
