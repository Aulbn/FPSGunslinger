﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private ZombieController zombieParent;

    public void SetInfo(ZombieController zombieParent)
    {
        this.zombieParent = zombieParent;
    }

    public void Damage(float damage)
    {
        zombieParent.currentHealth -= damage;
    }
}
