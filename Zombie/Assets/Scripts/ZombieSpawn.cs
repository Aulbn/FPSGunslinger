using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ZombieSpawn : MonoBehaviour
{
    public static List<ZombieSpawn> AllSpawns;

    public float Radius { get { return transform.localScale.x / 2; } }

    //public int maxSpawnAmmount;

    private void Awake()
    {
        if (AllSpawns == null)
            AllSpawns = new List<ZombieSpawn>();
        AllSpawns.Add(this);
    }

    public static ZombieSpawn GetRandomSpawn()
    {
        return AllSpawns[Random.Range(0, AllSpawns.Count)]; 
    }

    public static Vector3 GetRandomSpawnpoint()
    {
        ZombieSpawn spawn = GetRandomSpawn();
        Vector3 spawnPos = spawn.transform.position + Random.insideUnitSphere * spawn.Radius;
        if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, spawn.Radius * 2, 1))
            spawnPos = hit.position;
        return spawnPos;
    }

    void OnDrawGizmosSelected()
    {
        Color color = Color.red;
        color.a = 0.5f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, Radius);
    }
}
