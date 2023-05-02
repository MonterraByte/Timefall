using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DummyScript : MonoBehaviour {
    private CharacterController characterController;

    private bool isHit = false;
    private float hitTime;
    private float currentTime;

    // Start is called before the first frame update
    void Start() {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update(){
        if (this.isHit && this.hitTime > currentTime) {
            this.currentTime += Time.deltaTime;
            characterController.enabled = false;
        }
        else if (this.isHit) {
            this.isHit = false;
            characterController.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 1 && !this.isHit) {
            this.currentTime = 0.0f;
            this.hitTime = 0.35f;
            this.isHit = true;
        }  
    }

    private void OnTriggerStay(Collider other) {
        /*if (other.gameObject.layer == 1 && !this.isHit)
        {
            this.currentTime = 0.0f;
            this.hitTime = 0.35f;
            this.isHit = true;
        }*/
    }
}
