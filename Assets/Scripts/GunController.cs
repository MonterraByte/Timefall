using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : RangedWeapon
{

    public Transform bulletSpawnPoint;
    public Transform bullet;


    //private bool LastDirection = false;
    



    public float bulletSpeed;


    public override void Fire()
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
