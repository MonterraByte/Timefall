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

    private GameObject player;

    private int chargePhase = 0;

    private bool isHit = false;

    public void Awake()
    {
        bulletLayer = LayerMask.NameToLayer("Enemy Projectile");
        player = FindObjectOfType<PlayerScript>().gameObject;
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
        this.chargePhase = 1;
        float elapsedTime = 0.0f;
        float duration = 5.0f;
        float stunDuration = 3.0f;
        float chargeDuration = 1.5f;
        Vector3 originalPos = this.transform.position;
        Vector3 finalPos = new Vector3(109.0f, originalPos.y + 1.25f, originalPos.z);
        Vector3 upPos = new Vector3(originalPos.x, originalPos.y + 1.25f, originalPos.z);
        float currentX = originalPos.x;
        float currentY = originalPos.y;

        while (elapsedTime < duration && !this.isHit)
        {
            float t = (elapsedTime / duration);
            this.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2.5f, t);
            this.transform.position = Vector3.Lerp(originalPos, upPos, t);
            elapsedTime += Time.deltaTime;
            chargeDuration = 1.5f + 3.5f * t;
            yield return null;
        }

        this.isHit = false;
        this.chargePhase = 2;
        elapsedTime = 0.0f;

        Debug.Log("pos x = " + this.transform.position.x);

        while (elapsedTime < chargeDuration && !this.isHit)
        {
            float t = (elapsedTime / chargeDuration);
            this.transform.position = Vector3.Lerp(upPos, finalPos, t);
            elapsedTime += Time.deltaTime;

            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (Math.Abs(distance) < 3.0f)
            {
                this.isHit = true;
                PlayerScript playerScript = player.GetComponent<PlayerScript>();
                playerScript.setHealth(0);
            }

            yield return null;
        }

        this.chargePhase = 3;
        elapsedTime = 0.0f;

        if (!this.isHit)
        {
            Vector3 originalScale = this.transform.localScale;

            while (elapsedTime < stunDuration)
            {
                float t = (elapsedTime / duration);
                this.transform.localScale = Vector3.Lerp(originalScale, Vector3.one, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        currentX = this.transform.position.x;
        currentY = this.transform.position.y;

        while (Mathf.Abs(currentX - originalPos.x) > 0.1f || Mathf.Abs(currentY - originalPos.y) > 0.1f)
        {
            currentX = Mathf.MoveTowards(currentX, originalPos.x, 100 * Time.deltaTime);
            currentY = Mathf.MoveTowards(currentY, originalPos.y, 100 * Time.deltaTime);
            transform.position = new Vector3(currentX, currentY, transform.position.z);

            yield return null;
        }

        //this.transform.position = originalPos;
        this.isHit = false;
        this.chargePhase = 0;

        this.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(2);
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

            if (this.chargePhase == 1) this.isHit = true;
            this.health -= 10;
           
        }
    }

}
