using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    public int typeCollectable;
    public CollectibleSpawner spawner;


    void Start(){
        typeCollectable = Random.Range(1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(90f, Time.time * 100f, 0);
    }


    void OnTriggerEnter(Collider other){
        Debug.Log(other.gameObject.name);
        Debug.Log(other.gameObject.tag);
        if(other.CompareTag("Player")){
            Debug.Log("match");
            HealthManager healthManager = GameObject.FindObjectOfType<HealthManager>();
            switch(typeCollectable){
                case 1:
                    healthManager.Lives++;
                    break;
                case 2:
                    healthManager.Shields++;
                    break;
                default:
                    Debug.LogError("Invalid collectable type.");
                    break;
            }
            spawner.collectableDestroyed();
            Destroy(gameObject);
        }
    }
}
