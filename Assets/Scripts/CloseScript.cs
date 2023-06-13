using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseScript : MonoBehaviour
{
    public Transform playerTransform;

    private bool done = false;
    private float startSpot;

    private GameObject childObject;
    // Start is called before the first frame update
    void Start()
    {
        this.startSpot = this.transform.position.x;
        this.childObject = this.transform.Find("SpawnPoint").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkClose() && !done)
        {
            this.transform.position += Vector3.right * 25 * Time.deltaTime;

            if (this.transform.position.x > this.startSpot + 5.5f)
            {
                this.done = true;
            }
        }
    }

    bool checkClose()
    {
        if (playerTransform.position.x > 82 && playerTransform.position.x < 110
            && playerTransform.position.y > 59 && playerTransform.position.y < 73)
        {
            RespawnManagerScript manager = GameObject.FindObjectOfType<RespawnManagerScript>();

            if (manager != null)
            {
                manager.setSpawnPoint(this.childObject);
            }

            return true;
        }
        return false;
    }
}
