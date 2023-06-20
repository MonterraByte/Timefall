using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

        randomMove = BossMove.ConeProjectile;

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
