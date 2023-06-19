using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponMenuController : MonoBehaviour
{
    private static readonly int OpenWeaponWheelAnimProperty = Animator.StringToHash("OpenWeaponWheel");

    public Animator anim;
    public static int weaponID;

    public RangedWeapon weapon;
    public InputActionReference toggleAction;


    private bool _isOpen;
    private bool isOpen {
        get => _isOpen;
        set {
            _isOpen = value;
            weaponID = 0;
            anim.SetBool(OpenWeaponWheelAnimProperty, _isOpen);
        }
    }

    private void Start() {
        toggleAction.action.started += _ => isOpen = !isOpen;  // TODO: make it hold instead of press
    }

    private void OnEnable() {
        toggleAction.action.Enable();
    }

    private void OnDisable() {
        toggleAction.action.Disable();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isOpen)
        {
            switch (weaponID)
            {
                case 0:// nothing is selected
                    break;
                case 1: //wrench
                    Debug.Log("Wrench");
                    isOpen = false;
                    break;
                case 2: //laser gun
                    Debug.Log("Laser gun");
                    weapon.disableGuns();
                    weapon.activateWeapon(0);
                    isOpen = false;
                    break;
                case 3: //flamethrower
                    Debug.Log("flamethrower");
                    if (weapon.checkIfPossible(2))
                    {
                        weapon.disableGuns();
                        weapon.activateWeapon(2);
                    }
                    isOpen = false;
                    break;
                case 4: //boomerang
                    Debug.Log("boomerang");
                    if (weapon.checkIfPossible(1))
                    {
                        weapon.disableGuns();
                        weapon.activateWeapon(1);
                    }
                    isOpen = false;
                    break;
            }
        }
    }
}
