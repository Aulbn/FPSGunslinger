using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamagable
{
    void OnDamage(float damage, Vector3 force);
}

public class Destructable : MonoBehaviour, IDamagable
{
    [Header("Weight")]
    public float weightInfluence = 0;
    private Rigidbody rb;

    [Header("Destruction")]
    public bool indestructable;
    public float health;
    public GameObject destructionPrefab;
    public UnityEvent onDamage;
    public UnityEvent onDestroy;

    public void OnDamage(float damage, Vector3 force)
    {
        if (!indestructable)
            health -= damage;

        if (!indestructable && health <= 0)
        {
            Instantiate(destructionPrefab, transform.position, Quaternion.identity);
            onDestroy.Invoke();
        }
        else
            Damage(force);
    }

    private void Damage(Vector3 force)
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(force * weightInfluence);
        }

        onDamage.Invoke();
    }

    public void Destroy(float time)
    {
        StartCoroutine(IEDestroy(time));
    }

    private IEnumerator IEDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
