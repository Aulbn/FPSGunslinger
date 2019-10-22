using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private NavMeshAgent agent;

    public float currentHealth;
    [SerializeField] private float maxHealth;

    public Animator animator;
    private bool ragdolling = false;
    private Collider[] colliders;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        colliders = GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            Hitbox hitbox = c.gameObject.AddComponent<Hitbox>();
            hitbox.SetInfo(this);
        }
        ToggleRagdoll(false);
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        ragdolling = isRagdoll;
        foreach (Collider c in colliders)
        {
            c.isTrigger = !isRagdoll;
            animator.enabled = !isRagdoll;
            c.attachedRigidbody.isKinematic = !isRagdoll;
            agent.enabled = !isRagdoll;
        }
    }

    //private void Update()
    //{
    //    if (currentHealth <= 0 && !ragdolling)
    //        ToggleRagdoll(true);
    //}
}
