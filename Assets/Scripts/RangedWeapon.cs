using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class RangedWeapon : MonoBehaviour
{

    // An array of FireMethod delegates
    private FireMethod[] weapons;

    public delegate void FireMethod();

    // The current weapon index
    private int currentWeapon = 0;


    public float CooldownDuration = 1.0f;

    private bool IsAvailable = true;

    //private bool LastDirection = false;

    public float SpeedRotation = 25f;


    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;

        // Get the references to each script and assign them to the array
        weapons = new FireMethod[3];
        weapons[0] = GetComponent<Gun>().Fire;
        weapons[1] = GetComponent<Boomerang>().Fire;
        weapons[2] = GetComponent<Flamethrower>().Fire;
    }

    private void Update()
    {
        RotateGun();

        // Check if the user presses Q
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            // Disable all weapon scripts first
            for (int i = 0; i < weapons.Length; i++)
            {
                ((MonoBehaviour)GetComponent(weapons[i].Method.DeclaringType)).enabled = false;
            }

            // Change the current weapon index by adding one and using modulo to wrap around
            currentWeapon = (currentWeapon + 1) % weapons.Length;

            // Enable the script for the current weapon
            ((MonoBehaviour)GetComponent(weapons[currentWeapon].Method.DeclaringType)).enabled = true;
        }


        // if not available to use (still cooling down) just exit
        if (getAvailable() == false)
        {
            return;
        }

        // Check if the user presses left mouse button
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Invoke the Fire method of the current weapon
            weapons[currentWeapon]();
        }


    }

    private void RotateGun()
    {

        // Get the player's input component
        PlayerInput playerInput = GetComponentInParent<PlayerInput>();

        // Get the horizontal input value
        float horizontalInput = playerInput.actions["Move"].ReadValue<Vector2>().x;

        // Determine the direction the player is moving in

        /*
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

        */



        Plane playerplane = new Plane(new Vector3(0, 0, 1), transform.position);
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        float hitdist;

        if (playerplane.Raycast(ray, out hitdist))
        {
            Vector3 targetPoint = ray.GetPoint(hitdist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
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

