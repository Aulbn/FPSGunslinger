using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int ownerID;
    public PlayerController Owner { get { return PlayerController.AllPlayers[ownerID]; } }
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

    [Header("VFX")]
    public ParticleSystem muzzleFlare;
    public ParticleSystem hitEffect;

    [Header("Animation")]
    public Vector3 armAimPos;
    public Vector3 armAimRot;

    private float fireRateTimer = 0;

    public bool CanShoot { get { return fireRateTimer <= 0 && currentAmmo > 0 && rememberShootTimer > 0; } }

    private void Start()
    {
        currentAmmo = magSize;
    }

    private void Update()
    {
        TimersCounter();
        //Debug.Log(fireRateTimer + " : " + rememberShootTimer);

        if (CanShoot)
            Shoot();
    }

    public void SetOwnerID(int playerID)
    {
        ownerID = playerID;
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

    public void TryShoot()
    {
        rememberShootTimer = rememberShootTime;
    }

    private void Shoot()
    {
        RaycastHit hit;

        muzzleFlare.Play(true);
        ResetShootTimers();

        if (Physics.Raycast(Owner.cam.transform.position, Owner.cam.transform.forward, out hit, fireRange))
        {
            ParticleSystem ps = Instantiate(hitEffect, hit.point, Quaternion.Euler(hit.normal)).GetComponent<ParticleSystem>();
            ps.transform.rotation = Quaternion.LookRotation(hit.normal);

            Hitbox hitbox = hit.transform.GetComponent<Hitbox>();
            if (hitbox != null)
                hitbox.Damage(damage, Owner.cam.transform.forward * damage * 700 );
        }
        Owner.ShootCallback(recoilAmmount);
    }
}
