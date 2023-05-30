using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    public static event Action OnDestroyed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("here power");
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            OnDestroyed?.Invoke();
        }
    }
}
