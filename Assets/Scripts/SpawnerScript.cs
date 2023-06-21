using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public float rotationSpeed = 1.0f;

    void Update()
    {
        this.transform.Rotate(Vector3.up * (this.rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            var player = other.gameObject.GetComponent<PlayerScript>();
            player.SetRespawnPoint(gameObject);
        }
    }
}
