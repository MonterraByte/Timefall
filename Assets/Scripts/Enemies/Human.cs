using UnityEngine;

public class Human : Enemy
{
    private Vector3 targetPosition;
    private float minimumMoveDistance = 1f;
    private float maximumMoveDistance = 5f;

    protected override void DefaultAction()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = GetRandomPoint();
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;

        // check for collisions with walls
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 0.5f))
        {
            if (hit.collider.gameObject.CompareTag("Boundarie"))
            {
                targetPosition = GetRandomPoint();
            }
        }
    }

    private Vector3 GetRandomPoint()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Vector3 randomPoint = transform.position + randomDirection * Random.Range(minimumMoveDistance, maximumMoveDistance);
        randomPoint.y = 0f;
        return randomPoint;
    }
}




