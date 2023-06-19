using UnityEngine;

public class Infected : Enemy
{
    public System.Action killed;  

    public int attackDamage = 10;

    protected override void AttackPlayer(){
        if (Time.time > lastAttackTime + attackCooldown) {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            playerScript.health -= attackDamage;
        }
        
    }
}