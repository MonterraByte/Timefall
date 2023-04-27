using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{

    public Transform bulletSpawnPoint;
    public Transform bullet;

    public float CooldownDuration = 1.0f;

    private bool IsAvailable = true;

    private bool LastDirection = false;
    



    public float bulletSpeed;

    public float SpeedRotation = 25f;


    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        RotateGun();

        // if not available to use (still cooling down) just exit
        if (IsAvailable == false)
        {
            return;
        }

        Fire();


    }

    private void RotateGun()
    {

        // Get the player's input component
        PlayerInput playerInput = GetComponentInParent<PlayerInput>();

        // Get the horizontal input value
        float horizontalInput = playerInput.actions["Move"].ReadValue<Vector2>().x;

        // Determine the direction the player is moving in
        if (horizontalInput > 0)
        {
            // Player is moving right
            LastDirection = false;
        }
        else if (horizontalInput < 0)
        {
            // Player is moving left
            LastDirection = true;

        }
        else
        {
            LastDirection = false;
            // Player is not moving horizontally

        }

        

        Plane playerplane = new Plane(new Vector3(0,0,1), transform.position);
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        float hitdist;

        if(playerplane.Raycast(ray, out hitdist))
        {
            Vector3 targetPoint = ray.GetPoint(hitdist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, SpeedRotation * Time.deltaTime);
        }
    }



    public IEnumerator StartCooldown()
    {
        IsAvailable = false;
        yield return new WaitForSeconds(CooldownDuration);
        IsAvailable = true;
    }


    public void Fire()
    {
        // Check if the left mouse button is pressed
        if (Mouse.current.leftButton.isPressed)
        {


            Transform bulletTrans = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody BulletRB = bulletTrans.GetComponent<Rigidbody>();
            BulletRB.AddRelativeForce(Vector3.forward * bulletSpeed);


            StartCoroutine(StartCooldown());
        }

        
    }



}
