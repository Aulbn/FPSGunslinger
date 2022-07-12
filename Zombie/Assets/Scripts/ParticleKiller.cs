using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleKiller : MonoBehaviour
{
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps)
            StartCoroutine(ParticleDestroy(ps));
        VisualEffect vfx = GetComponent<VisualEffect>();
        if (vfx)
            StartCoroutine(VFXDestroy(vfx));
    }

    public IEnumerator ParticleDestroy(ParticleSystem ps)
    {
        while (ps.IsAlive(true))
        {
            Debug.DrawRay(ps.transform.position, ps.transform.forward);
            yield return null;
        }
        Destroy(gameObject);
    }

    private IEnumerator VFXDestroy(VisualEffect vfx)
    {
        bool hasSpawnedParticles = false;
        while(true)
        {
            if (vfx.aliveParticleCount > 0)
                hasSpawnedParticles = true;

            if (hasSpawnedParticles && vfx.aliveParticleCount == 0)
                break;

            yield return null;
        }
        Destroy(gameObject);
    }
}
