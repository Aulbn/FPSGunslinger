using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private ZombieController zombieParent;

    public void SetInfo(ZombieController zombieParent)
    {
        this.zombieParent = zombieParent;
    }

    public void Damage(float damage, Vector3 force)
    {
        zombieParent.currentHealth -= damage;

        if (zombieParent.currentHealth <= 0)
        {
            zombieParent.Die();
            GetComponent<Rigidbody>().AddForce(force);
        }
    }
}
