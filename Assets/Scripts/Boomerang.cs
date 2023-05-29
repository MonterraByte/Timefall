using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Boomerang : RangedWeapon
{
    public Transform boomerangSpawnPoint;
    public Transform boomerang;

    public float boomerangSpeed;
    public float boomerangRange;
    public float boomerangReturnSpeed;

    private Transform boomerangInstance;

    public override void Fire()
    {

            // Instantiate a boomerang object
            boomerangInstance = Instantiate(boomerang, boomerangSpawnPoint.position, boomerangSpawnPoint.rotation);
            // Get the rigidbody component of the boomerang object
            Rigidbody boomerangRB = boomerangInstance.GetComponent<Rigidbody>();
            // Add a force to make the boomerang fly forward
            boomerangRB.AddRelativeForce(Vector3.forward * boomerangSpeed);
            // Add a torque to make the boomerang spin
            boomerangRB.AddRelativeTorque(Vector3.up * boomerangSpeed);
            // Add a script to handle the return logic
            BoomerangReturnHandler returnHandler = boomerangInstance.gameObject.AddComponent<BoomerangReturnHandler>();
            // Pass the parameters for range and return speed
            returnHandler.boomerangRange = boomerangRange;
            returnHandler.boomerangReturnSpeed = boomerangReturnSpeed;
            // Pass a reference to the player transform
            returnHandler.playerTransform = transform.parent;

            StartCoroutine(StartCooldown());
        
    }
}
