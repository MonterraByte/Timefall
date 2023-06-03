using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class RangedWeapon : MonoBehaviour
{

    public float CooldownDuration = 1.0f;

    private bool IsAvailable = true;

    //private bool LastDirection = false;

    private bool FacingRight = true;

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
        if (getAvailable() == false)
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
            //LastDirection = false;
            FacingRight = true;
        }
        else if (horizontalInput < 0)
        {
            // Player is moving left
            //LastDirection = true;
            FacingRight = false;
        }
        /*else
        {
            LastDirection = false;
            // Player is not moving horizontally

        }

        */



        Plane playerplane = new Plane(new Vector3(0, 0, 1), transform.position);
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        float hitdist;

        if (playerplane.Raycast(ray, out hitdist))
        {
            Vector3 targetPoint = ray.GetPoint(hitdist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

            if (FacingRight && targetRotation.y < 0) // Facing right and pointing left
            {
                targetRotation.y = -targetRotation.y;
                targetRotation.z = -targetRotation.z;
            }
            else if (!FacingRight && targetRotation.y > 0) // Facing left and pointing right
            {
                targetRotation.y = -targetRotation.y;
                targetRotation.z = -targetRotation.z;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, SpeedRotation * Time.deltaTime);
        }
    }


    public bool getAvailable()
    {

        return IsAvailable;

    }

    public IEnumerator StartCooldown()
    {
        IsAvailable = false;
        yield return new WaitForSeconds(CooldownDuration);
        IsAvailable = true;
    }


    public virtual void Fire()
    {


    }



}

