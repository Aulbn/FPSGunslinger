using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    PlayerController owner { get { return PlayerController.Player; } }
    [Header("Stats")]
    public float damage;
    public float fireRate;
    public float fireRange;
    public float recoilAmmount;

    private float rememberShootTimer = 0;
    private float rememberShootTime = 0.1f;
    private IEnumerator ShootRemember;

    [Header("Ammo")]
    public int currentAmmo;
    public int magSize;
    public float reloadTime;

    [Header("VFX")]
    public VisualEffect muzzleFlash;
    public VisualEffect hitEffect;
    public AudioClip gunSound;
    private AudioSource source;

    [Header("Recoil")]
    public AnimationCurve recoil; //Behöver nog ändra sen med automatvapen
    public float recoilForce, recoilTime;

    private Animator animator;

    private float fireRateTimer = 0;
    private bool isReloading;

    public bool CanShoot { get { return fireRateTimer <= 0 && currentAmmo > 0 && rememberShootTimer > 0 && !isReloading; } }
    private Action OnShoot;

    private void Start()
    {
        currentAmmo = magSize;
        UIManager.SetAmmoText(currentAmmo, magSize);

        source = gameObject.AddComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        TimersCounter();

        if (CanShoot)
            Shoot();
    }

    private void TimersCounter()
    {
        if (rememberShootTimer > 0)
            rememberShootTimer -= Time.deltaTime;
        if (fireRateTimer > 0)
            fireRateTimer -= Time.deltaTime;
    }

    private void ResetShootTimers()
    {
        fireRateTimer = fireRate;
        rememberShootTimer = 0;
    }

    public void TryShoot(Action onShoot)
    {
        rememberShootTimer = rememberShootTime;
        OnShoot = onShoot;
    }

    private void Shoot()
    {
        RaycastHit hit;

        muzzleFlash.Play();
        source.PlayOneShot(gunSound, 0.5f);
        ResetShootTimers();
        if (animator)
            animator.SetTrigger("Fire");

        currentAmmo--;

        if (Physics.Raycast(owner.cam.transform.position, owner.cam.transform.forward, out hit, fireRange))
        {
            VisualEffect ps = Instantiate(hitEffect, hit.point, Quaternion.Euler(hit.normal)).GetComponent<VisualEffect>();
            ps.transform.rotation = Quaternion.LookRotation(hit.normal);

            DecalPool.SpawnDecal(hit);

            IDamagable damagable = hit.transform.GetComponent<IDamagable>();
            if (damagable != null)
                damagable.OnDamage(damage, owner.cam.transform.forward * damage * 700 );
        }

        OnShoot();
        UIManager.SetAmmoText(currentAmmo, magSize);
        StartCoroutine(IERecoil(owner.cam.transform));
    }

    public bool Reload()
    {
        if (isReloading || currentAmmo >= magSize) return false;
        if (animator)
            animator.SetTrigger("Reload");
        StartCoroutine(IEReload());
        return true;
    }

    private IEnumerator IEReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        currentAmmo = magSize;
        UIManager.SetAmmoText(currentAmmo, magSize);
    }

    private IEnumerator IERecoil(Transform cam)
    {
        float timer = 0;
        while (timer < recoilTime)
        {
            timer += Time.deltaTime;
            cam.transform.localEulerAngles = new Vector3(-recoil.Evaluate(timer / recoilTime) * recoilForce, 0);
            yield return new WaitForEndOfFrame();
        }
    }
}
