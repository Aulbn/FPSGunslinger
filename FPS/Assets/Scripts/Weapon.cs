using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    public float damage;
    public float fireRate;
    public float fireRange;

    [Header("Ammo")]
    public int currentAmmo;
    public int magSize;

    [Header("VFX")]
    public ParticleSystem muzzleFlare;
    public ParticleSystem hitEffect;

    private float fireRateTimer = 0;

    public bool CanShoot { get { return fireRateTimer >= fireRate && currentAmmo > 0; } }

    private void Start()
    {
        currentAmmo = magSize;
    }

    private void Update()
    {
        FireRateCounter();
    }

    public bool Shoot(Camera cam)
    {
        if (!CanShoot) return false;

        RaycastHit hit;

        muzzleFlare.Play(true);
        fireRateTimer = 0;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, fireRange))
        {
            ParticleSystem ps = Instantiate(hitEffect, hit.point, Quaternion.Euler(hit.normal)).GetComponent<ParticleSystem>();
            ps.transform.rotation = Quaternion.LookRotation(hit.normal);
        }
        return true;
    }

    private void FireRateCounter()
    {
        if (fireRateTimer < fireRate)
            fireRateTimer += Time.deltaTime;
    }
}
