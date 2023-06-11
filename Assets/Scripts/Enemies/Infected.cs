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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            movementSpeed = 0f;
        }
    }

    
}