using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    public List<Transform> spawnPoints = new List<Transform>();

    //Called when scene is done loading locally
    public override void SceneLoadLocalDone(string scene)
    {
        GameObject[] mapSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        for (int i = 0; i < mapSpawnPoints.Length; i++)
        {
            spawnPoints.Add(mapSpawnPoints[i].transform);
        }

        //Position to spawn player at
        Vector3 spawnPos = Vector3.zero;
        if (spawnPoints.Count > 0)
        {
            spawnPos = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        }
        else
        {
            spawnPos = Vector3.zero;
        }

        //Spawning the player
        BoltNetwork.Instantiate(BoltPrefabs.TestPlayer, spawnPos, Quaternion.identity);
    }
}
