using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flamethrower : RangedWeapon
{
    public Transform fireSpawnPoint;
    public ParticleSystem fire;

    public float fireDamage;
    public float fireDuration;

    private ParticleSystem currentFirePS;

    public override void Fire()
    {

            // Instantiate a particle system that simulates fire
            currentFirePS = Instantiate(fire, fireSpawnPoint.position, fireSpawnPoint.rotation, fireSpawnPoint); // Set the parent to the fire spawn point
            currentFirePS.transform.localPosition = Vector3.zero; // Set the local position to zero
            currentFirePS.transform.localRotation = Quaternion.identity; // Set the local rotation to zero
            // Get the collider component of the particle system
            //ParticleSystemCollider fireCollider = currentFirePS.GetComponent<ParticleSystemCollider>();
            // Set the trigger property to true so that it can detect collisions
            //fireCollider.enabled = true;
            // Add a script to handle collisions and damage
            FireCollisionHandler fireHandler = currentFirePS.gameObject.AddComponent<FireCollisionHandler>();
            // Pass the parameters for damage and duration
            fireHandler.fireDamage = fireDamage;
            fireHandler.fireDuration = fireDuration;
            fireHandler.fire = currentFirePS;

            StartCoroutine(StartCooldown());
        

    }

}

