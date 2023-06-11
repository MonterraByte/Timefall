using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float platformHeight = 0f;
    public int damage = 10;
    public float movementSpeed = 5f;
    public float attackRange = 1f;
    public float sightRange = 1f;
    public float attackCooldown = 2f;
    public float speedRotation = 25f;
    protected int health = 10;

    protected Transform player;
    protected float lastAttackTime = 0f;

    private void Start()
    {
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
    }

    protected void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = platformHeight;
        transform.position += direction * movementSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), speedRotation * Time.deltaTime);

    }

    protected bool CanAttack()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange && Time.time > lastAttackTime + attackCooldown;
    }

    protected virtual void AttackPlayer()
    {
    }

    private void OnTriggerEnter(Collider other) {
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