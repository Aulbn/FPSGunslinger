using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Zombies")]
    public GameObject zombiePrefab;
    public float zombieSpawnRate;
    public bool spawnZombies;
    //public Terrain terrain;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(ZombieSpawner());
    }

    private IEnumerator ZombieSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(zombieSpawnRate);
            Vector3 playerPos = PlayerController.Instance.transform.position;
            float offset = Random.Range(3f, 10f);
            if (spawnZombies)
                SpawnZombie(new Vector3 (Random.Range(playerPos.x -offset, playerPos.x + offset), playerPos.y, Random.Range(playerPos.z - offset, playerPos.z + offset)));
        }
    }

    private void SpawnZombie(Vector3 spawnPos)
    {
        if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 50f, 1))
            spawnPos = hit.position;
        Instantiate(zombiePrefab, spawnPos, Quaternion.LookRotation(PlayerController.Instance.transform.position - spawnPos));
    }

}
