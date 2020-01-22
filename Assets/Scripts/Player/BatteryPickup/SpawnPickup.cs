using UnityEngine;
using System.Collections.Generic;

public class SpawnPickup : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject spawnObj;


    private void Awake()
    {
        Spawn();
    }

    void Spawn()
    {
        GameObject[] worldSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        for (int i = 0; i < worldSpawnPoints.Length; i++)
        {
            spawnPoints.Add(worldSpawnPoints[i].transform);
        }
        Vector3 spawnPos = Vector3.zero;
        if (spawnPoints.Count > 0)
        {
            spawnPos = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        }
        else
        {
            spawnPos = Vector3.zero;
        }

        Instantiate(spawnObj, spawnPos, Quaternion.identity);

    }


}
