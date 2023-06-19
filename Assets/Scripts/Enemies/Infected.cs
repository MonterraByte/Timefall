using UnityEngine;

public class Infected : Enemy
{
    public System.Action killed;  

    public int attackDamage = 10;

    //player


    protected override void AttackPlayer(){
        if (Time.time > lastAttackTime + attackCooldown) {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            playerScript.DamagePlayer(attackDamage);
        }
        
    }

    // if infected in contact with player, infected stops moving
    protected override void OnTriggerEnter(Collider other)
    {
        // Call the base method to handle bullet collision
        base.OnTriggerEnter(other);
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

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            Rigidbody enemyRigidbody = GetComponent<Rigidbody>();
            enemyRigidbody.velocity = Vector3.zero;
            enemyRigidbody.angularVelocity = Vector3.zero;
        }
    }
}