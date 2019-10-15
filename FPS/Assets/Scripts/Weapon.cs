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

    public bool Shoot()
    {
        if (!CanShoot) return false;

        Debug.Log("Shoot!");
        fireRateTimer = 0;
        return true;
    }

    private void FireRateCounter()
    {
        if (fireRateTimer < fireRate)
            fireRateTimer += Time.deltaTime;
    }
}
