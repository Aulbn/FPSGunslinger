using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Player;

    [Header("Input")]
    public float walkSpeed = 2f;
    public float jumpStrength = 5f;
    public float gravity = 20f;
    public float mouseSensitivity = 1f;
    private PlayerControls controls;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool shootInput;
    public Vector3 Velocity { get; private set; }

    [Header("Weapon")]
    public Weapon activeWeapon;
    public bool IsAiming { get; private set; }
    public float aimFOV;
    private float defaultFOV;
    public float aimZoomSpeed;
    public float AimValue { get; private set; }

    [Header("References")]
    public Animator armsAnim;
    public Transform head;
    public Camera cam;
    private float headRotY;

    private CharacterController cc;
    private bool isPaused = false;
    private PlayerInput playerInput;

    void Awake()
    {
        if (!Player) Player = this;
        else Destroy(gameObject);

        cc = GetComponent<CharacterController>();

        bool uh = false;
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions.FindAction("Shoot", uh).performed += ctx => shootInput = true;
        playerInput.actions.FindAction("Shoot", uh).canceled += ctx => shootInput = false;

        OnPause();//KÄND BUG - Måste börja med UI-actionmappen
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    public void OnLook (InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnAimDown()
    {
        AimToggle();
    }

    public void OnJump()
    {
        if (cc.isGrounded)
        {
            Vector3 vel = Velocity;
            vel.y = jumpStrength;
            Velocity = vel;
        }
    }

    public void OnReload()
    {
        if (activeWeapon.Reload())
            armsAnim.SetTrigger("Reload");
    }

    public void OnPause()
    {
        //playerInput.SwitchCurrentActionMap(isPaused ? "Gameplay" : "UI");
        //movementInput = Vector2.zero;
        //lookInput = Vector2.zero;
        //PlayerUI.ToggleMenu();
        //isPaused = !isPaused;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        defaultFOV = cam.fieldOfView;

        SetPosition(GameManager.Instance.playerSpawn.position);
    }

    void Update()
    {
        AimUpdate();

        if (shootInput)
            activeWeapon.TryShoot(ShootCallback); //ALSO DO RECOIL
    }

    private void FixedUpdate()
    {
        MovementUpdate();
        RotationUpdate();
    }

    public void SetPosition(Vector3 position)
    {
        cc.enabled = false;
        cc.transform.position = position;
        Velocity = Vector3.zero;
        cc.enabled = true;
    }

    private void RotationUpdate()
    {
        Vector2 input = lookInput * mouseSensitivity;
        transform.Rotate(transform.up * input.x);

        headRotY += input.y;
        ClampRotation();
    }

    private void ClampRotation()
    {
        headRotY = Mathf.Clamp(headRotY, -90f, 90f);
        head.localRotation = Quaternion.Euler(-headRotY, 0f, 0f);
    }

    private void MovementUpdate()
    {
        if (cc.isGrounded)
            Velocity = Velocity.y * transform.up + (movementInput.y * transform.forward + movementInput.x * transform.right) * walkSpeed;
        else
            Velocity += Vector3.down * gravity * Time.deltaTime; //Gravity

        cc.Move(Velocity * Time.deltaTime); //Movement
        armsAnim.SetBool("IsWalking", movementInput != Vector2.zero);
    }

    private void ShootCallback()
    {
        armsAnim.SetTrigger("Shoot");
    }

    private void AimToggle()
    {

        IsAiming = !IsAiming;
        armsAnim.SetBool("IsAiming", IsAiming);
        UIManager.ToggleCrosshair(!IsAiming, 0.1f);
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
        //armsCam.fieldOfView = Mathf.Lerp(defaultFOV, aimFOV, AimValue);
        //armsAnim.transform.localEulerAngles = Vector3.Lerp(defaultArmAimRot, activeWeapon.armAimRot, AimValue);
        //armsAnim.transform.localPosition = Vector3.Lerp(defaultArmAimPos, activeWeapon.armAimPos, AimValue);
        //PlayerUI.SetCrosshairOpacity(Mathf.Abs(AimValue-1));
        armsAnim.SetFloat("AimValue", AimValue);
    }
}
