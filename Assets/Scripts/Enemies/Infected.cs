using UnityEngine;

public class Infected : Enemy
{

    private bool isCured = false;
    public System.Action killed;    

    protected override void TakeDamage(){
        health -= damage;
        if (health <= 0){
            if (this.killed != null){
                this.killed.Invoke();
            }
            Die();
            

        }
    }

    protected override void AttackPlayer(){
        
    }

    protected void Cure(){
        isCured = true;
    }
    
}