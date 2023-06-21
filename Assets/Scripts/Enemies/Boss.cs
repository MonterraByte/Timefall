using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Boss : MonoBehaviour {
    public int initialHealth = 500;

    private int _health;
    public int Health {
        get => _health;
        set {
            _health = value;
            if (_health < 0) {
                OnDeath();
            }
        }
    }

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

    private void OnEnable() {
        Health = initialHealth;
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
        float chargeDuration = 2.5f;
        Vector3 originalPos = this.transform.position;
        Vector3 finalPos = new Vector3(109.0f, originalPos.y, originalPos.z);
        Vector3 upPos = new Vector3(originalPos.x, originalPos.y + 1.25f, originalPos.z);
        float currentX = originalPos.x;
        float currentY = originalPos.y;

        while (elapsedTime < duration && !this.isHit)
        {
            var t = elapsedTime / duration;
            this.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2.5f, t);
            finalPos = new Vector3(109.0f, Mathf.Lerp(originalPos.y ,originalPos.y + 1.25f, t), originalPos.z);
            upPos = new Vector3(originalPos.x, Mathf.Lerp(originalPos.y ,originalPos.y + 1.25f, t), originalPos.z);
            this.transform.position = Vector3.Lerp(originalPos, upPos, t);
            elapsedTime += Time.deltaTime;
            chargeDuration = 2.5f + 1.5f * t;
            yield return null;
        }

        this.isHit = false;
        this.chargePhase = 2;
        elapsedTime = 0.0f;

        while (elapsedTime < chargeDuration && !this.isHit)
        {
            float t = (elapsedTime / chargeDuration);
            this.transform.position = Vector3.Lerp(upPos, finalPos, t);
            elapsedTime += Time.deltaTime;

            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (Math.Abs(distance) < 2.5f)
            {
                this.isHit = true;
                PlayerScript playerScript = player.GetComponent<PlayerScript>();
                playerScript.TakeDamage(1);
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
        const float flySpeed = 3f; // Adjust the fly speed as desired
        const float initialX = 85f; // Initial x position to fly towards
        const float totalDuration = 20f; // Total duration of the action

        var initialPosition = transform.position;
        float targetY = initialPosition.y + 5f;
        float currentX = initialPosition.x;
        float currentY = initialPosition.y;

        const float bulletCooldownTime = 0.5f;
        var bulletCooldown = 0.0f;

        // Fly towards initialX and elevated position
        while (Mathf.Abs(currentX - initialX) > 0.1f || Mathf.Abs(currentY - targetY) > 0.1f)
        {
            currentX = Mathf.MoveTowards(currentX, initialX, flySpeed * 2 * Time.deltaTime);
            currentY = Mathf.MoveTowards(currentY, targetY, flySpeed * 2 * Time.deltaTime);
            transform.position = new Vector3(currentX, currentY, transform.position.z);


            // Create a bullet object
            bulletCooldown -= Time.deltaTime;
            if (bulletCooldown <= 0.0f) {
                var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                bullet.gameObject.layer = bulletLayer;


                var BulletRB = bullet.GetComponent<Rigidbody>();
                BulletRB.AddRelativeForce(-bullet.transform.up * bulletSpeed);
                bulletCooldown = bulletCooldownTime;
            }

            yield return null;
        }

        // Reset variables for back-and-forth movement
        var previousX = initialX;
        var targetX = previousX + 20f;
        currentX = transform.position.x;
        currentY = transform.position.y;

        var elapsedTime = 0f;
        // Back-and-forth movement
        while (elapsedTime < totalDuration)
        {
            currentX = Mathf.MoveTowards(currentX, targetX, flySpeed * Time.deltaTime);
            transform.position = new Vector3(currentX, currentY, transform.position.z);

            if (Mathf.Abs(currentX - targetX) <= 0.1f)
            {
                (targetX, previousX) = (previousX, targetX);
                yield return new WaitForSeconds(1f);
            }

            // Create a bullet object
            bulletCooldown -= Time.deltaTime;
            if (bulletCooldown <= 0.0f) {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                bullet.gameObject.layer = bulletLayer;


                var BulletRB = bullet.GetComponent<Rigidbody>();
                BulletRB.AddRelativeForce(-bullet.transform.up * bulletSpeed);
                bulletCooldown = bulletCooldownTime;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return to the original position
        var finalPosition = transform.position;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime) {
            transform.position = Vector3.Lerp(finalPosition, initialPosition, t);
            yield return null;
        }

        chooseAttack();
        yield break;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Wrench")) {
            if (this.chargePhase == 1) this.isHit = true;
            Health -= 10;
        }
    }

    protected void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("FlameThrow")) {
            if (this.chargePhase == 1) this.isHit = true;
            Health -= 10;
        }
    }

    private void OnDeath() {
        StopAllCoroutines();
        StartCoroutine(DeathAnimation());
    }

    private IEnumerator DeathAnimation() {
        var renderer = GetComponent<Renderer>();
        var initialColor = renderer.material.color;
        var targetColor = Color.red;
        const float duration = 3.0f;
        for (var t = 0.0f; t < duration; t += Time.deltaTime) {
            renderer.material.color = Color.Lerp(initialColor, targetColor, t * (1.0f/duration));
            yield return null;
        }

        renderer.enabled = false;

        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("Credits");
    }
}
