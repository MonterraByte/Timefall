using UnityEngine;

public class Bot : Enemy
{

    //Patrol
    private Transform[] patrolPoints;

    public void SetPatrolPoints(Transform[] patrolPoints)
    {
        this.patrolPoints = patrolPoints;
    }

    private int currentPatrolIndex = 0;

    protected override void DefaultAction()
    {
        if (patrolPoints == null || patrolPoints.Length == 0){
            return;
        }

        //patrol
        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.1f){
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
        Vector3 direction = (patrolPoints[currentPatrolIndex].position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), speedRotation * Time.deltaTime);

    }

    protected override void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 movement = direction * movementSpeed * Time.deltaTime;
        characterController.Move(movement);
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);
    }
}

