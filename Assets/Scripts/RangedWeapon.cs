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

    public void disableGuns()
    {
        // Disable all weapon scripts first
        for (int i = 0; i < this.weapons.Length; i++)
        {
            Debug.Log(weapons.Length);
            ((MonoBehaviour)GetComponent(this.weapons[i].Method.DeclaringType)).enabled = false;
        }

    }

    public void activateWeapon(int current)
    {

        this.currentWeapon = current;

        Debug.Log(this.currentWeapon);

        // Enable the script for the current weapon
        ((MonoBehaviour)GetComponent(this.weapons[this.currentWeapon].Method.DeclaringType)).enabled = true;
    }

    private void Update()
    {
        RotateGun();


        // if not available to use (still cooling down) just exit
        if (getAvailable() == false)
        {
            return;
        }

        // Check if the user presses left mouse button
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log(this.currentWeapon);
            weapons[this.currentWeapon]();
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

