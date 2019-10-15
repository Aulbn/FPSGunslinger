using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float mouseSensitivity = 1f;

    public Weapon activeWeapon;
    public Animator armsAnim;
    public bool IsAiming { get; private set; }
    public float aimFOV;
    private float defaultFOV;
    public float aimZoomSpeed;

    private CharacterController cc;
    private Camera cam;
    private float camRotY;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        defaultFOV = cam.fieldOfView;
    }

    void Update()
    {
        MovementUpdate();
        RotationUpdate();
        AimUpdate();

        if (Input.GetKeyDown(KeyCode.Mouse1))
            AimToggle();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            TryShoot();
    }

    private void RotationUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitivity;
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
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        cc.SimpleMove((input.z * transform.forward + input.x * transform.right) * walkSpeed);
    }

    private void TryShoot()
    {
        if (activeWeapon == null) return;
        if (activeWeapon.Shoot())
        {
            armsAnim.SetTrigger("Shoot");
            camRotY += 5f; //Recoil ammount
            ClampRotation();
        }
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
            if (cam.fieldOfView > aimFOV)
                cam.fieldOfView -= aimZoomSpeed * Time.deltaTime;
            else cam.fieldOfView = aimFOV;
        }
        else
        {
            if (cam.fieldOfView < defaultFOV)
                cam.fieldOfView += aimZoomSpeed * Time.deltaTime;
            else cam.fieldOfView = defaultFOV;
        }
    }
}
