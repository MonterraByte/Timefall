using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseScript : MonoBehaviour
{
    public Transform playerTransform;

    private bool done = false;
    private float startSpot;
    // Start is called before the first frame update
    void Start()
    {
        this.startSpot = this.transform.position.x;
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
            return true;
        }
        return false;
    }
}
