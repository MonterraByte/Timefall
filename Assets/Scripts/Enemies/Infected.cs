using UnityEngine;

public class Infected : Enemy
{
    public System.Action killed;  

    protected override void AttackPlayer(){
        if (Time.time > lastAttackTime + attackCooldown) {
            Debug.Log("Attack");
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            playerScript.TakeDamage(damage);
        }
        
    }
}