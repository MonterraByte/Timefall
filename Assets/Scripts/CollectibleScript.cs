using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectibleScript : MonoBehaviour
{
    public static event Action onCollected;
    public int typeCollectable;


    void Start(){
        System.Random random = new System.Random();
        typeCollectable = random.Next(1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(90f, Time.time * 100f, 0);
        
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            onCollected?.Invoke();
            HealthManager healthManager = GameObject.FindObjectOfType<HealthManager>();
            switch(typeCollectable){
                case 1:
                    if(healthManager.getLifes() < 3){
                        healthManager.gainHeart();
                    }
                    break;
                case 2:
                    if(healthManager.getShields() == 0){
                        healthManager.gainShield();
                    }
                    break;
            }
            Destroy(gameObject);
        }
    }
}
