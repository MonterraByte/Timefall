using UnityEngine;

public class Human : Enemy
{

    protected override void AttackPlayer(){
        base.AttackPlayer();
        //gravity
        if (!characterController.isGrounded){
            Vector3 movement = Vector3.zero;
            movement.y += gravityValue * Time.deltaTime;
            characterController.Move(movement);
        }
    }

}






