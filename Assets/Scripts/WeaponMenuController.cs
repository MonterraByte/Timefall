using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponMenuController : MonoBehaviour
{

    public Animator anim;
    public static int weaponID;

    public RangedWeapon weapon;


    private bool weaponMenuSelected = false;

    // Update is called once per frame
    void Update()
    {

        // Check if the user presses Q
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            weaponID= 0;
            weaponMenuSelected = !weaponMenuSelected;

        }

        if (weaponMenuSelected)
        {

            anim.SetBool("OpenWeaponWheel", true);

            switch (weaponID)
            {
                case 0:// nothing is selected

                    break;

                case 1: //wrench
                    Debug.Log("Wrench");
                    weaponMenuSelected= false;
                    break;
                case 2: //laser gun
                    Debug.Log("Laser gun");
                    weapon.disableGuns();
                    weapon.activateWeapon(0);
                    weaponMenuSelected = false;
                    break;
                case 3: //flamethrower
                    Debug.Log("flamethrower");
                    weapon.disableGuns();
                    weapon.activateWeapon(2);
                    weaponMenuSelected = false;
                    break;
                case 4: //boomerang
                    Debug.Log("boomerang");
                    weapon.disableGuns();
                    weapon.activateWeapon(1);
                    weaponMenuSelected = false;
                    break;

                default: break;


            }

        }
        else
        {

            anim.SetBool("OpenWeaponWheel", false);

        }



    }
}
