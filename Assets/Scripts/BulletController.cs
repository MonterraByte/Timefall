using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{    
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }   

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
