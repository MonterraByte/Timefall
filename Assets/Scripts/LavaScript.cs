using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void detectCollision(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();

            int newHealth = (int)(player.health - 3 * Time.deltaTime);

            player.setHealth(newHealth);
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Infected"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        detectCollision(other);
    }

    private void OnTriggerStay(Collider other)
    {
        detectCollision(other);
    }
}
