using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayerHandling
{
    public BoltEntity character;
    public BoltConnection connection;

    public List<Transform> spawnPoints = new List<Transform>();

    public void Spawn()
    {
        GameObject[] mapSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        for (int i = 0; i < mapSpawnPoints.Length; i++)
        {
            spawnPoints.Add(mapSpawnPoints[i].transform);
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

        //TODO: FIX THIS SPAWNING SYNCING
        if (!character)
        {
            if (IsServer)
            {
                character = BoltNetwork.Instantiate(BoltPrefabs.NetGhost, spawnPos, Quaternion.identity);
                character.TakeControl();
            }
            else
            {
                character = BoltNetwork.Instantiate(BoltPrefabs.NetPlayer, spawnPos, Quaternion.identity);
                character.AssignControl(connection);
            }
        }

        character.transform.position = spawnPos;
    }

    public bool IsServer
    {
        get { return connection == null; }
    }

    public bool IsClient
    {
        get { return connection != null; }
    }
}