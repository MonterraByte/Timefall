using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponMenuController : MonoBehaviour
{
    private static readonly int OpenWeaponWheelAnimProperty = Animator.StringToHash("OpenWeaponWheel");

    public Animator anim;
    public static int weaponID;

    public RangedWeapon[] weapons;
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

    private bool Equip(int weaponId) {
        Debug.Log(weaponId);
        if (!weapons[weaponId].enabled) {
            return false;
        }

        for (var i = 0; i < weapons.Length; i++) {
            weapons[i].Equipped = i == weaponId;
        }
        return true;
    }

    private void Update()
    {
        if (isOpen)
        {
            if (weaponID == 0) {
                return;
            }
            Debug.Log(weaponID);

            var equippedWeapon = Equip(weaponID - 1);
            if (equippedWeapon) {
                isOpen = false;
            }
        }
    }
}
