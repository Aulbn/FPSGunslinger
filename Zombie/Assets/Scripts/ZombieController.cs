using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private NavMeshAgent agent;

    public float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private SkinnedMeshRenderer skin;

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
        SetDissolve(0, 0);
    }

    public void Die()
    {
        ToggleRagdoll(true);
        StartCoroutine(Dissolve(3f));
    }

    private void ToggleRagdoll(bool isRagdoll)
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

    private void SetDissolve(float ammount, float height)
    {
        Vector2 dissolveHeight = new Vector2(.8f, -1f); //Arbitrary

        skin.material.SetFloat("_Dissolve", ammount);
        skin.material.SetFloat("_DissolveHeight", Mathf.Lerp(dissolveHeight.x, dissolveHeight.y, height));
    }

    private IEnumerator Dissolve(float time)
    {
        float currentTime = 0;
        float val;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            val = currentTime / time;
            SetDissolve(val* .5f, val);
            yield return null;
        }
        Destroy(gameObject);
    }

    //private void Update()
    //{
    //    if (currentHealth <= 0 && !ragdolling)
    //        ToggleRagdoll(true);
    //}
}
