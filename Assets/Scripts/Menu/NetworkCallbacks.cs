using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    private void Awake()
    {
        NetworkPlayerRegistry.CreateServerPlayer();
    }

    public override void SceneLoadLocalDone(string scene)
    {
        NetworkPlayerRegistry.ServerPlayer.Spawn();
    }

    public override void Connected(BoltConnection connection)
    {
        NetworkPlayerRegistry.CreateClientPlayer(connection);
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        NetworkPlayerRegistry.GetClientPlayer(connection).Spawn();
    }

    public override void Disconnected(BoltConnection connection)
    {
        if (connection == NetworkPlayerRegistry.ServerPlayer.connection)
        {
            SceneManager.LoadScene(0);
            BoltNetwork.Shutdown();
        }
    }

    //Called when scene is done loading locally
    //public override void SceneLoadLocalDone(string scene)
    //{
    //    GameObject[] mapSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

    //    for (int i = 0; i < mapSpawnPoints.Length; i++)
    //    {
    //        spawnPoints.Add(mapSpawnPoints[i].transform);
    //    }

    //    //Position to spawn player at
    //    Vector3 spawnPos = Vector3.zero;
    //    if (spawnPoints.Count > 0)
    //    {
    //        spawnPos = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
    //    }
    //    else
    //    {
    //        spawnPos = Vector3.zero;
    //    }

    //    //Spawning the player
    //    if (BoltNetwork.IsServer)
    //    {
    //        BoltNetwork.Instantiate(BoltPrefabs.NetGhost, spawnPos, Quaternion.identity);
    //    }
    //    else
    //    {
    //        BoltNetwork.Instantiate(BoltPrefabs.NetPlayer, spawnPos, Quaternion.identity);
    //    }
    //}

}
