using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using System.Linq;

public class DecalPool : MonoBehaviour
{
    private static DecalPool Instance;
    private Queue<DecalProjector> decals = new Queue<DecalProjector>();

    public GameObject decalPrefab;
    public int maxAmmount;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    public static void SpawnDecal(RaycastHit hit)
    {
        Vector3 pos = hit.point + hit.normal * 0.5f;
        DecalProjector decal;

        do
        {
            if (Instance.decals.Count < Instance.maxAmmount)
                decal = Instantiate(Instance.decalPrefab).GetComponent<DecalProjector>();
            else
                decal = Instance.decals.Dequeue();
        } while (!decal);

        decal.transform.position = pos;
        decal.transform.rotation = Quaternion.LookRotation(-hit.normal);
        decal.transform.parent = hit.transform;
        Instance.decals.Enqueue(decal);
    }

}
