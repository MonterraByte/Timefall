using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    public int typeCollectable;
    public CollectibleSpawner spawner;


    void Start(){
        typeCollectable = Random.Range(1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(90f, Time.time * 100f, 0);
    }


    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")) {
            var player = other.gameObject.GetComponent<PlayerScript>();
            switch(typeCollectable){
                case 1:
                    player.Lives++;
                    break;
                case 2:
                    player.Shields++;
                    break;
                default:
                    Debug.LogError("Invalid collectable type.");
                    break;
            }
            spawner.collectableDestroyed();
            Destroy(gameObject);
        }
    }
}
