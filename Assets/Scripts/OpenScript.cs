using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScript : MonoBehaviour
{
    public float moveSpeed = 20.0f;
    public float limitRight = 0.0f;

    private bool start = false;
    // Start is called before the first frame update
    void Start()
    {
        PowerUpScript.OnDestroyedFlame += HandleObjectDestroyed;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            this.transform.position += Vector3.right * this.moveSpeed * Time.deltaTime;

            if (this.transform.position.x >= this.limitRight)
            {
                this.start = false;
                PowerUpScript.OnDestroyedFlame -= HandleObjectDestroyed;
            }
        }
    }

    private void HandleObjectDestroyed()
    {
        this.start = true;
    }
}
