using UnityEngine;

public class Bot : Enemy
{

    //Patrol
    private Transform[] patrolPoints;

    //Attack
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 0.1f;

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

    protected override void AttackPlayer()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Vector3 direction = (player.position - bullet.transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), speedRotation * Time.deltaTime);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = direction * bulletSpeed;
    }

}

