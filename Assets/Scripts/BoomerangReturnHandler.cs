using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangReturnHandler : MonoBehaviour
{
    public float boomerangRange; // The maximum distance that the boomerang can travel
    public float boomerangReturnSpeed; // The speed at which the boomerang returns

    public Transform playerTransform; // A reference to the player's transform

    private Vector3 startPosition; // The initial position of the boomerang
    private Vector3 endPosition; // The final position of the boomerang
    private float distanceTraveled; // The distance traveled by the boomerang
    private bool isReturning; // A flag to indicate if the boomerang is returning

    private void Start()
    {
        startPosition = transform.position; // Set the start position to the current position
        endPosition = playerTransform.position; // Set the end position to the player's position
        distanceTraveled = 0f; // Initialize the distance traveled to zero
        isReturning = false; // Initialize the flag to false
    }

    private void FixedUpdate()
    {
        if (!isReturning) // Check if the boomerang is not returning
        {
            distanceTraveled = Vector3.Distance(startPosition, transform.position); // Calculate the distance traveled by using the start position and current position
            if (distanceTraveled >= boomerangRange) // Check if the distance traveled has reached or exceeded the range
            {
                isReturning = true; // Set the flag to true
            }
        }
        else // If the boomerang is returning
        {
            endPosition = playerTransform.position; // Update the end position to follow the player's position
            transform.position = Vector3.Lerp(transform.position, endPosition, boomerangReturnSpeed * Time.deltaTime); // Lerp from current position to end position by using a speed factor and delta time
            if (Vector3.Distance(transform.position, endPosition) < 0.1f) // Check if the boomerang has reached or is close enough to the end position
            {
                Destroy(gameObject); // Destroy the boomerang object
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collider belongs to an obstacle or an enemy
        if (collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Destroy the boomerang object
        }

    }

}
