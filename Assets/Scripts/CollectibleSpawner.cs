using System.Collections;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject collectibleBox;
    public float delay = 4;

    private bool canSpawn = true;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(spawnCollectable());
    }


    private IEnumerator spawnCollectable(){
        while(true){
            if(canSpawn){
                spawn();
            }
            yield return new WaitForSeconds(delay);
        }
    }

    public void collectableDestroyed(){
        canSpawn = true;
    }

    public void spawn(){
        var spawnObject = Instantiate(collectibleBox, this.transform.position, Quaternion.Euler(90f, Time.time * 100f, 0), this.transform);
        spawnObject.GetComponent<CollectibleScript>().spawner = this;
        canSpawn = false;
    }
}
