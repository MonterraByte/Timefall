using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Boss : MonoBehaviour
{
    public int health = 100;

    public float bulletSpeed = 500.0f;

    public GameObject bulletPrefab;

    private static readonly BossMove[] moves = (BossMove[])Enum.GetValues(typeof(BossMove));

    private int bulletLayer;

    public void Awake()
    {
        bulletLayer = LayerMask.NameToLayer("Enemy Projectile");
    }


    public enum BossMove
    {
        ConeProjectile,
        AllDirectionsProjectile,
        Charge,
        FlyAndDropProjectiles
    }

    private void Start()
    {
        chooseAttack();
    }

    private void chooseAttack()
    {

        // Choose a random attack move
        BossMove randomMove = moves[UnityEngine.Random.Range(0, moves.Length)];

        randomMove = BossMove.FlyAndDropProjectiles;

        switch (randomMove)
        {
            case BossMove.ConeProjectile:
                Debug.Log("Switch case : Cone Projectile");
                StartCoroutine(ConeProjectileAttack());

                break;
            case BossMove.AllDirectionsProjectile:
                StartCoroutine(AllDirectionsAttack());
                break;
            case BossMove.Charge:
                StartCoroutine(ChargeAttack());
                break;
            case BossMove.FlyAndDropProjectiles:
                StartCoroutine(FlyAndDropAttack());
                break;
        }

    }


    private IEnumerator ConeProjectileAttack()
    {
        const int bulletCount = 10;
        const float delayBetweenBullets = 1f; // Adjust the delay as desired

        for (int i = 0; i < bulletCount; i++)
        {

            // Calculate the angle for each bullet
            // Generate a random angle within the desired range
            float angle = UnityEngine.Random.Range(-22.5f, 22.5f);

            // Create a bullet object
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            bullet.gameObject.layer = bulletLayer;
            // Rotate the bullet towards the calculated angle
            bullet.transform.rotation = Quaternion.Euler(0f,0f, angle);

            var BulletRB = bullet.GetComponent<Rigidbody>();
            BulletRB.AddRelativeForce(bullet.transform.right * bulletSpeed);

            yield return new WaitForSeconds(delayBetweenBullets);
        }

        yield return new WaitForSeconds(3);
        chooseAttack();
        yield break;
    }
    private IEnumerator AllDirectionsAttack()
    {
        const int bulletCount = 10;
        const float delayBetweenBullets = 0.1f; // Adjust the delay as desired

        for (int i = 0; i < bulletCount; i++)
        {

            // Calculate the angle for each bullet
            float angle = (-45f / 2f) + (45f / (bulletCount - 1)) * i;

            // Create a bullet object
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            bullet.gameObject.layer = bulletLayer;
            // Rotate the bullet towards the calculated angle
            bullet.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            var BulletRB = bullet.GetComponent<Rigidbody>();
            BulletRB.AddRelativeForce(bullet.transform.right * bulletSpeed);

            yield return new WaitForSeconds(delayBetweenBullets);
        }

        yield return new WaitForSeconds(2);
        chooseAttack();
        yield break;
    }

    private IEnumerator ChargeAttack()
    {
        chooseAttack();
        yield break;
    }
    private IEnumerator FlyAndDropAttack()
    {
        const float flySpeed = 40f; // Adjust the fly speed as desired
        const float initialX = 85f; // Initial x position to fly towards
        const float totalDuration = 20f; // Total duration of the action

        float originalX;
        float targetX;
        float originalY = transform.position.y;
        float targetY = originalY + 5f;
        float currentX = transform.position.x;
        float currentY = transform.position.y;
        float elapsedTime = 0f;

        // Fly towards initialX and elevated position
        while (Mathf.Abs(currentX - initialX) > 0.1f || Mathf.Abs(currentY - targetY) > 0.1f)
        {
            currentX = Mathf.MoveTowards(currentX, initialX, flySpeed * 2 * Time.deltaTime);
            currentY = Mathf.MoveTowards(currentY, targetY, flySpeed * 2 * Time.deltaTime);
            transform.position = new Vector3(currentX, currentY, transform.position.z);


            // Create a bullet object
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            bullet.gameObject.layer = bulletLayer;


            var BulletRB = bullet.GetComponent<Rigidbody>();
            BulletRB.AddRelativeForce(-bullet.transform.up * bulletSpeed);

            elapsedTime += Time.deltaTime;

            yield return new WaitForSeconds(0.5f);

            
        }

        // Reset variables for back-and-forth movement
        originalX = initialX;
        targetX = originalX + 20f;
        currentX = transform.position.x;
        currentY = transform.position.y;
        elapsedTime = 0f;

        // Back-and-forth movement
        while (elapsedTime < totalDuration)
        {
            currentX = Mathf.MoveTowards(currentX, targetX, flySpeed * Time.deltaTime);
            transform.position = new Vector3(currentX, currentY, transform.position.z);

            if (Mathf.Abs(currentX - targetX) <= 0.1f)
            {
                float tempX = targetX;
                targetX = originalX;
                originalX = tempX;



                yield return new WaitForSeconds(2f);
            }

            // Create a bullet object
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            bullet.gameObject.layer = bulletLayer;


            var BulletRB = bullet.GetComponent<Rigidbody>();
            BulletRB.AddRelativeForce(-bullet.transform.up * bulletSpeed);

            elapsedTime += Time.deltaTime;

            yield return new WaitForSeconds(0.5f);
            
        }

        // Return to the original position
        while (Mathf.Abs(currentX - originalX) > 0.1f && Mathf.Abs(currentY - originalY) > 0.1f)
        {
            currentX = Mathf.MoveTowards(currentX, originalX, flySpeed * Time.deltaTime);
            currentY = Mathf.MoveTowards(currentY, originalY, flySpeed * Time.deltaTime);
            transform.position = new Vector3(currentX, currentY, transform.position.z);

            yield return null;
        }

        chooseAttack();
        yield break;
    }







    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            this.health -= 10;
           
        }
    }

}
