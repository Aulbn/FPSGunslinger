using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Players")]
    public Transform playerSpawn;

    [Header("Zombies")]
    public GameObject zombiePrefab;
    public bool spawnZombies;

    [Header("Waves")]
    public float waveTime = 60;
    public int spawnAmmount = 5;
    public float zombieSpawnRate = 3f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(WaveCounter()); //Starta inte direkt, kanske. Ha någon introfas
    }

    private IEnumerator ZombieSpawner(int waveSize)
    {
        //Debug.Log("Start Spawning Zombies!");
        for (int i = waveSize; i > 0; i--)
        {
            if (spawnZombies)
            {
                SpawnZombie(ZombieSpawn.GetRandomSpawnpoint()); //Spawn zombie AND SEND UP NORMAL ASWELL
            }
            yield return new WaitForSeconds(zombieSpawnRate);
        }
    }


    private IEnumerator WaveCounter()
    {
        float waveTimer;
        while (true)
        {
            waveTimer = waveTime;

            StartCoroutine(ZombieSpawner(spawnAmmount)); //Start wave!

            while (waveTimer > 0)
            {
                waveTimer -= Time.deltaTime;
                UIManager.SetTimerText(waveTimer);
                yield return null;
            }
        }
    }

    private void SpawnZombie(Vector3 spawnPos)
    {
        Instantiate(zombiePrefab, spawnPos, Quaternion.LookRotation(PlayerController.Player.transform.position - spawnPos)); //Rotate along up normal so they don't crawl up weird (if that happens idk)
    }

    

}
