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

    public override void Fire()
    {
        // Check if the left mouse button is pressed
        if (Mouse.current.rightButton.isPressed)
        {
            // Instantiate a particle system that simulates fire
            ParticleSystem firePS = Instantiate(fire, fireSpawnPoint.position, fireSpawnPoint.rotation);
            // Get the collider component of the particle system
            
            //ParticleSystemCollider fireCollider = firePS.GetComponent<ParticleSystemCollider>();
            // Set the trigger property to true so that it can detect collisions
            //fireCollider.trigger = true;
            // Add a script to handle collisions and damage
            FireCollisionHandler fireHandler = firePS.gameObject.AddComponent<FireCollisionHandler>();
            // Pass the parameters for damage and duration
            fireHandler.fireDamage = fireDamage;
            fireHandler.fireDuration = fireDuration;

            StartCoroutine(StartCooldown());
        }
    }
}
