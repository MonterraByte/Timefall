using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{

    public GameObject collectibleBox;
    public float delay = 10;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount == 0){
            timer += Time.deltaTime;
            if(timer > delay){
                Instantiate(collectibleBox, this.transform.position, Quaternion.Euler(90f, Time.time * 100f, 0), this.transform);
            }
        }

    }
}
