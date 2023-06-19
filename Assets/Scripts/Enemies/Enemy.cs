using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 10;
    public float movementSpeed = 5f;
    public float attackRange = 1f;
    public float sightRange = 1f;
    public float attackCooldown = 2f;
    public float speedRotation = 25f;
    protected int health = 10;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 0.1f;

    protected Transform player;
    protected float lastAttackTime = 0f;

    private float gravityValue = -9.81f;

    protected CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (gameObject.tag == "Infected")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (gameObject.tag == "Human")
        {  
            gameObject.GetComponent<Renderer>().material.color = Color.white;   
        }
        else if (gameObject.tag == "Bot")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    protected virtual void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 movement = direction * movementSpeed * Time.deltaTime;

        movement.y += gravityValue * Time.deltaTime;

        characterController.Move(movement);

        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);

        if (characterController.isGrounded)
        {
            movement.y = 0f;
        }
    }
    
    private void Update()
    {
        if (Vector3.Distance(transform.position, player.position) > sightRange)
        {
            DefaultAction();
        }
        else if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            ChasePlayer();
        }
        else
        {
            if (CanAttack())
            {
                AttackPlayer();

                lastAttackTime = Time.time;
            }
        }
    }

    protected virtual void DefaultAction()
    {
        Vector3 movement = Vector3.zero;
        
        movement.y += gravityValue * Time.deltaTime;

        characterController.Move(movement);

        if (characterController.isGrounded)
        {
            movement.y = 0f;
        }
    }

    protected bool CanAttack()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange && Time.time > lastAttackTime + attackCooldown;
    }

    protected virtual void AttackPlayer()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Vector3 direction = (player.position - bullet.transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), speedRotation * Time.deltaTime);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = direction * bulletSpeed;
    }

    protected virtual void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Bullet") {
            DamageEnemy(other.gameObject.GetComponent<BulletController>().damage);
        }
    }

    protected virtual void DamageEnemy(int damage){
        TakeDamage(damage);
        VerifyPlayerHealth();
    }

    private void TakeDamage(int damage){
        health -= damage;
    }

    private void VerifyPlayerHealth() {
        if (health <= 0) {
            Die();
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}