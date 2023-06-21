using UnityEngine;

public class LavaScript : MonoBehaviour {
    private void detectCollision(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            player.TakeDamage(1, true);
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
