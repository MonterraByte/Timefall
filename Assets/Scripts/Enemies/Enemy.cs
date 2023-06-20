using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 10;
    public float movementSpeed = 5f;
    public float attackRange = 1f;
    public float sightRange = 1f;
    public float attackCooldown = 2f;
    protected float lastAttackTime = 0f;
    public float speedRotation = 25f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 0.1f;
    public float durationStun = 1.0f;

    protected Transform player;
    protected float gravityValue = -9.81f;
    protected CharacterController characterController;
    protected Animator animator;
    private int projectileLayer;

    private bool isHooked = false;
    private Vector2 hookDir;
    private float hookSpeed;
    private bool whichSide;
    private float limitX;

    private bool isStunned = false;
    private float currentStunDuration = 0.0f;

    private void Start()
    {
        projectileLayer = LayerMask.NameToLayer("Enemy Projectile");
        characterController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
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

    protected void Update()
    {
        if (this.isHooked)
        {
            Hooked();
        }
        else if (this.isStunned)
        {
            Stunned();
            DefaultAction();
        }
        else if (Vector3.Distance(transform.position, player.position) > sightRange)
        {
            DefaultAction();
        }
        else if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            ChasePlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    protected virtual void Hooked()
    {
        Vector3 movement;

        if (this.whichSide) movement = new Vector3(this.hookDir.x, -this.hookDir.y, 0) * this.hookSpeed * Time.deltaTime;
        else movement = new Vector3(-this.hookDir.x, -this.hookDir.y, 0) * this.hookSpeed * Time.deltaTime;

        characterController.Move(movement);

        if (Mathf.Abs(this.transform.position.x - this.limitX) < 1.0f)
        {
            this.isHooked = false;
            this.isStunned = true;
            this.currentStunDuration = 0.0f;
        }
    }

    protected virtual void Stunned()
    {
        if (this.currentStunDuration > this.durationStun)
        {
            this.isStunned = false;
        }
        else
        {
            this.currentStunDuration += Time.deltaTime;
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
        if (!CanAttack())
        {
            return;
        }
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.layer = projectileLayer;
        Vector3 direction = (player.position - bullet.transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), speedRotation * Time.deltaTime);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = direction * bulletSpeed;
        lastAttackTime = Time.time;
    }

    protected virtual void OnTriggerEnter(Collider other){
        Debug.Log("Getting in: " + other.gameObject.tag);

        switch(other.gameObject.tag)
        {
            case "Bullet":
                Destroy(gameObject);
                break;

            case "HookWeapon":
                GotHooked(other.gameObject);
                break;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Getting in: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("ForceField"))
        {
            Destroy(gameObject);
        }
    }

    private void GotHooked(GameObject stick)
    {
        this.isHooked = true;
        HookshotScript hookshotScript = stick.GetComponent<HookshotScript>();
        this.hookDir = hookshotScript.getCurrentCoordinates();
        this.hookSpeed = hookshotScript.hookSpeed;
        this.whichSide = hookshotScript.getSide();
        this.limitX = FindObjectOfType<PlayerScript>().transform.position.x;
    }
}
