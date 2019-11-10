using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //public static PlayerController Instance;

    public static List<PlayerController> AllPlayers;
    public int playerID;

    [Header("Input")]
    public float walkSpeed = 2f;
    public float jumpStrength = 5f;
    public float gravity = 20f;
    public float mouseSensitivity = 1f;
    private PlayerControls controls;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool shootInput;

    [Header("References")]
    public Weapon activeWeapon;
    public Animator armsAnim;
    public bool IsAiming { get; private set; }
    public float aimFOV;
    private float defaultFOV;
    public float aimZoomSpeed;
    //public float AimValue { get { return Mathf.Abs(((cam.fieldOfView - aimFOV) / (defaultFOV - aimFOV)) - 1); } }
    public float AimValue { get; private set; }
    public Vector3 Velocity { get; private set; }

    private Vector3 defaultArmAimPos, defaultArmAimRot;


    private CharacterController cc;
    public Camera cam;
    public Camera armsCam;
    private float camRotY;

    void Awake()
    {
        if (AllPlayers == null)
            AllPlayers = new List<PlayerController>();
        playerID = AllPlayers.Count;
        AllPlayers.Add(this);

        cc = GetComponent<CharacterController>();

        controls = new PlayerControls();
        //controls.Gameplay.Jump.performed += ctx => Jump();
        controls.Gameplay.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Shoot.started += ctx => shootInput = true;
        controls.Gameplay.Shoot.canceled += ctx => shootInput = false;
        controls.Gameplay.AimDown.performed += ctx => AimToggle();



        var player1 = controls.Gameplay;
        var player2 = controls.Gameplay;
        //player1.Jump.performed += ctx => Jump();

        //var player2 = player1.Clone();
        //InputActionRebindingExtensions.ApplyBindingOverridesOnMatchingControls(controls.Gameplay, Gamepad.all[1]);
        //PlayerInputManager
        //print("Specified player " + playerID);

        //print (controls.devices);

        //foreach (Gamepad g in Gamepad.all)
        //{
        //    print("Gamepad: " + g.displayName + " | " + g.shortDisplayName);
        //}

        //foreach (InputDevice g in InputDevice.all)
        //{
        //    print("InputDevice: " + g.displayName + " | " + g.shortDisplayName);
        //}

    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
        //controls.bindingMask = new InputBinding()
        //InputActionRebindingExtensions.ApplyBindingOverridesOnMatchingControls(controls.Gameplay.Move, )
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void Start()
    {
        defaultArmAimPos = armsAnim.transform.localPosition;
        defaultArmAimRot = armsAnim.transform.localEulerAngles;
        Cursor.lockState = CursorLockMode.Locked;
        defaultFOV = cam.fieldOfView;
    }

    void Update()
    {
        AimUpdate();

        //if (Input.GetKeyDown(KeyCode.Mouse1))
        //    AimToggle();

        if (shootInput)
            activeWeapon.TryShoot();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
        RotationUpdate();
    }

    private void Jump()
    {
        if (cc.isGrounded)
        {
            Vector3 vel = Velocity;
            vel.y = jumpStrength;
            Velocity = vel;
        }
    }

    private void RotationUpdate()
    {
        Vector2 input = lookInput * mouseSensitivity;
        //lookInput *= mouseSensitivity;
        transform.Rotate(transform.up * input.x);

        camRotY += input.y;
        ClampRotation();
    }

    private void ClampRotation()
    {
        camRotY = Mathf.Clamp(camRotY, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(-camRotY, 0f, 0f);
    }

    private void MovementUpdate()
    {
        //Vector3 vel = Velocity;
        if (cc.isGrounded)
        {
            Velocity = Velocity.y* transform.up + (movementInput.y * transform.forward + movementInput.x * transform.right) * walkSpeed;
        }
        else
        {
            Velocity += Vector3.down * gravity * Time.deltaTime; //Gravity
        }
        cc.Move(Velocity * Time.deltaTime); //Movement
    }

    public void ShootCallback(float recoilAmmount)
    {
        armsAnim.SetTrigger("Shoot");
        camRotY += recoilAmmount;
        ClampRotation();
    }

    private void AimToggle()
    {

        IsAiming = !IsAiming;
        armsAnim.SetBool("Aiming", IsAiming);
    }

    private void AimUpdate()
    {
        if (IsAiming)
        {
            if (AimValue < 1)
                AimValue += aimZoomSpeed * Time.deltaTime;
            else AimValue = 1;
        }
        else
        {
            if (AimValue > 0)
                AimValue -= aimZoomSpeed * Time.deltaTime;
            else AimValue = 0;
        }

        cam.fieldOfView = Mathf.Lerp(defaultFOV, aimFOV, AimValue);
        armsCam.fieldOfView = Mathf.Lerp(defaultFOV, aimFOV, AimValue);
        armsAnim.transform.localEulerAngles = Vector3.Lerp(defaultArmAimRot, activeWeapon.armAimRot, AimValue);
        armsAnim.transform.localPosition = Vector3.Lerp(defaultArmAimPos, activeWeapon.armAimPos, AimValue);
        UIManager.SetCrosshairOpacity(Mathf.Abs(AimValue-1));
        armsAnim.SetFloat("AimValue", AimValue);
    }
}
