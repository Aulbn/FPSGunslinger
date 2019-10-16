using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKiller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ParticleDestory());
    }

    public IEnumerator ParticleDestory()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        while (ps.IsAlive(true))
        {
            Debug.DrawRay(ps.transform.position, ps.transform.forward);
            yield return null;
        }
        Destroy(gameObject);
    }
}
