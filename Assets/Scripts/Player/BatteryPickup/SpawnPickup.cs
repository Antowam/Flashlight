using UnityEngine;
using System.Collections.Generic;

public class SpawnPickup : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject spawnObj;


    private void Awake()
    {
        SpawnBattery();
    }

    private void Update()
    {
        RespawnSpawnBatteryPickup();
    }

    void SpawnBattery()
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
    void RespawnSpawnBatteryPickup()
    {
        if(GameObject.FindGameObjectWithTag("Battery") == null)
        {
            Debug.Log("No battery pickup found, spawning new pickup");
            SpawnBattery();
        }
    }
}
