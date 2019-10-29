using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public float walkSpeed = 2f;
    public float mouseSensitivity = 1f;

    public Weapon activeWeapon;
    public Animator armsAnim;
    public bool IsAiming { get; private set; }
    public float aimFOV;
    private float defaultFOV;
    public float aimZoomSpeed;
    //public float AimValue { get { return Mathf.Abs(((cam.fieldOfView - aimFOV) / (defaultFOV - aimFOV)) - 1); } }
    public float AimValue { get; private set; }
    private Vector3 defaultArmAimPos, defaultArmAimRot;

    private CharacterController cc;
    private Camera cam;
    public Camera armsCam;
    private float camRotY;

    public static Camera Camera { get { return Instance.cam; } }

    void Awake()
    {
        Instance = this;
        cc = GetComponent<CharacterController>();
        cam = Camera.main;
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
        MovementUpdate();
        RotationUpdate();
        AimUpdate();

        if (Input.GetKeyDown(KeyCode.Mouse1))
            AimToggle();

        if (Input.GetKey(KeyCode.Mouse0))
            activeWeapon.TryShoot();
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

    public static void ShootCallback(float recoilAmmount)
    {
        Instance.armsAnim.SetTrigger("Shoot");
        Instance.camRotY += recoilAmmount;
        Instance.ClampRotation();
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
