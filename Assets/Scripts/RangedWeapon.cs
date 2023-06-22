using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public abstract class RangedWeapon : MonoBehaviour {
    public InputActionReference fireAction;
    public InputActionReference positionalAimAction;
    public InputActionReference directionalAimAction;

    public float CooldownDuration = 1.0f;

    private bool _equipped;
    public bool Equipped {
        get => _equipped;
        set {
            _equipped = value;
            CanFire = _equipped;
        }
    }
    private bool CanFire = true;

    private bool FacingRight = true;

    public float SpeedRotation = 25f;


    private Camera mainCamera;

    private enum AimDevice {
        Mouse,
        Controller,
    }
    private AimDevice lastUsedDevice;
    private Vector2 lastMousePosition = Vector2.zero;
    private Vector2 lastControllerDirection = Vector2.zero;

    protected abstract void Fire();

    private void Start()
    {
        mainCamera = Camera.main;

        fireAction.action.started += ctx => {
            if (ctx.started) {
                OnFire();
            }
        };
        fireAction.action.Enable();
        positionalAimAction.action.Enable();
        directionalAimAction.action.Enable();
    }

    private void OnFire() {
        if (!CanFire) {
            return;
        }
        CanFire = false;
        Fire();
    }

    private void Update() {
        var mousePosition = positionalAimAction.action.ReadValue<Vector2>();
        var controllerDirection = directionalAimAction.action.ReadValue<Vector2>();
        lastUsedDevice = (lastMousePosition - mousePosition).sqrMagnitude >
                         (lastControllerDirection - controllerDirection).sqrMagnitude ? AimDevice.Mouse : AimDevice.Controller;
        lastMousePosition = mousePosition;
        lastControllerDirection = controllerDirection;

        RotateGun();
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

        Vector3? rotationDirection = null;
        if (lastUsedDevice == AimDevice.Mouse || lastControllerDirection == Vector2.zero) {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            var playerplane = new Plane(new Vector3(0, 0, 1), transform.position);
            float hitdist;
            if (playerplane.Raycast(ray, out hitdist)) {
                var targetPoint = ray.GetPoint(hitdist);
                rotationDirection = targetPoint - transform.position;
            }
        }
        else {
            rotationDirection = lastControllerDirection;
        }

        if (rotationDirection == null) {
            return;
        }

        var targetRotation = Quaternion.LookRotation(rotationDirection.Value);
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

        transform.rotation =
            Quaternion.Slerp(transform.rotation, targetRotation, SpeedRotation * Time.deltaTime);
    }

    public IEnumerator StartCooldown()
    {
        CanFire = false;
        yield return new WaitForSeconds(CooldownDuration);
        CanFire = true;
    }
}
