using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DummyScript : MonoBehaviour {
    private BoxCollider boxCollider;

    private bool isHit = false;
    private float hitTime;
    private float currentTime;
    private bool hitRight;

    // Start is called before the first frame update
    void Start() {
        this.boxCollider = GetComponent<BoxCollider>(); 
    }

    // Update is called once per frame
    void Update(){
        if (this.isHit && this.hitTime > currentTime) {
            this.currentTime += Time.deltaTime;
            if (hitRight) this.transform.position += Vector3.left * 10 * Time.deltaTime;
            else this.transform.position += Vector3.right * 10 * Time.deltaTime;
        }
        else if (this.isHit) {
            this.isHit = false;
        }

        this.transform.position += Vector3.right * Time.deltaTime;
        this.boxCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 1 && !this.isHit) {
            GotHit(other);
        }  
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == 1 && !this.isHit)
        {
            GotHit(other);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 2 && !this.isHit)
        {
            GotHit(collision);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 2 && !this.isHit)
        {
            GotHit(collision);
        }
    }

    private void GotHit(Collision collision)
    {
        this.currentTime = 0.0f;
        this.hitTime = 0.35f;
        this.isHit = true; ;
        if (collision.transform.position.x > transform.position.x) this.hitRight = true;
        else this.hitRight = false;
    } 
}
