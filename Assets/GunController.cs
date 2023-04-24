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
        
        Plane playerplane = new Plane(new Vector3(0,0,1), transform.position);
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        float hitdist;

        if(playerplane.Raycast(ray, out hitdist))
        {
            Vector3 targetPoint = ray.GetPoint(hitdist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 25 * Time.deltaTime);
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
