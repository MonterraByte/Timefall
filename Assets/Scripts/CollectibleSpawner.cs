using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{

    public GameObject collectibleBox;
    public float delay = 4;
    public bool isSpawn = true;


    // Update is called once per frame
    void Start()
    {
        StartCoroutine(spawnCollectable());
    }


    private IEnumerator spawnCollectable(){
        while(true){
            if(isSpawn){
                spawn();
            }
            yield return new WaitForSeconds(delay);
        }
        
    }

    public void collectableDestroyed(){
        isSpawn = true;
    }

    public void spawn(){
        var spawnObject = Instantiate(collectibleBox, this.transform.position, Quaternion.Euler(90f, Time.time * 100f, 0), this.transform);
        spawnObject.GetComponent<CollectibleScript>().spawner = this;
        isSpawn = false;
    }
}
