using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DummyScript : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update(){

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 1) {
            this.transform.position += Vector3.down * 0.1f;
        }  
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == 1) {
            this.transform.position += Vector3.down * 0.1f;
        }
    }
}
