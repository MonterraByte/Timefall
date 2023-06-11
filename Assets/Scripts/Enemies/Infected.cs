using UnityEngine;

public class Infected : Enemy
{

    private bool isCured = false;
    public System.Action killed;  

    public int attackDamage = 10;

    protected override void AttackPlayer(){
        
    }

    protected void Cure(){
        isCured = true;
    }

    // if infected in contact with player, infected stops moving
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (transform.position.x < player.position.x) {
                transform.position = new Vector3(transform.position.x - 3.0f, transform.position.y, transform.position.z);
            }
            else {
                transform.position = new Vector3(transform.position.x + 3.0f, transform.position.y, transform.position.z);
            }
        }
    }

    
}