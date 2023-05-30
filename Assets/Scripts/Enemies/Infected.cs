using UnityEngine;

public class Infected : Enemy
{

    private bool isCured = false;
    public System.Action killed;    

    protected override void AttackPlayer(){
        
    }

    protected void Cure(){
        isCured = true;
    }
    
}