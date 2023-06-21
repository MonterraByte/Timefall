using UnityEngine;

public class Infected : Enemy
{
    private static readonly int attackTrigger = Animator.StringToHash("Attack");
    private static readonly int walkingParameter = Animator.StringToHash("Walking");

    public System.Action killed;

    protected override void AttackPlayer(){
        if (Time.time > lastAttackTime + attackCooldown) {
            animator.SetTrigger(attackTrigger);
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            playerScript.TakeDamage(damage);
        }
    }

    private new void Update() {
        base.Update();
        animator.SetBool(walkingParameter, characterController.velocity.sqrMagnitude > 0.001);
    }
}
