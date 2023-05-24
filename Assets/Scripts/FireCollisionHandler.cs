using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class FireCollisionHandler : MonoBehaviour
{
    public float fireDamage; // The amount of damage per second
    public float fireDuration; // The duration of the fire effect
    public ParticleSystem fire;

    private float timer; // A timer to keep track of the fire duration

    private void Start()
    {
        timer = 0f; // Initialize the timer to zero
    }

    private void Update()
    {
        timer += Time.deltaTime; // Increment the timer by the elapsed time
        if (timer >= fireDuration || Mouse.current.rightButton.wasReleasedThisFrame) // Check if the timer has reached the fire duration
        {
            fire.Stop(true); // Destroy the fire particle
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        /*
        // Check if the collider belongs to an enemy
        if (other.CompareTag("Enemy"))
        {
            // Get the enemy script component
            Enemy enemy = other.GetComponent<Enemy>();
            // Apply damage over time to the enemy
            enemy.TakeDamage(fireDamage * Time.deltaTime);
            // Set the enemy on fire
            enemy.SetOnFire(true);
        }

        */
    }
}
