using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingScript : MonoBehaviour
{
    public bool isHorizontal = true;
    public bool goBack = false;
    public float moveLength = 10.0f;
    public float moveSpeed = 1.0f;

    private float startingPoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (isHorizontal) startingPoint = this.transform.position.x;
        else startingPoint = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isHorizontal)
        {
            this.transform.position += Vector3.right * this.moveSpeed * Time.deltaTime;

            if (((this.transform.position.x < startingPoint || this.transform.position.x > startingPoint + moveLength) && !this.goBack) ||
                ((this.transform.position.x > startingPoint || this.transform.position.x < startingPoint + moveLength) && this.goBack))
            {
                this.moveSpeed *= -1;
            }
        }
        else
        {
            this.transform.position += Vector3.up * this.moveSpeed * Time.deltaTime;

            if (((this.transform.position.y < startingPoint || this.transform.position.y > startingPoint + moveLength) && !this.goBack) ||
                ((this.transform.position.y > startingPoint || this.transform.position.y < startingPoint + moveLength) && this.goBack))
            {
                this.moveSpeed *= -1;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(this.transform);
        }
    }
    // && other.transform.position.x > this.transform.position.x
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }
}
