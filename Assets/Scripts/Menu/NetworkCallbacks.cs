using UnityEngine;

[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    //Positions optimized for "Room" scene
    Vector3[] spawnPositions = {
        new Vector3(-1f, 1f, -10f),
        new Vector3(-1f, 1f, -10f),
        new Vector3(-11f, 1f, -1f),
        new Vector3(-11f, 1f, -10f),
    };

    //Called when scene is done loading locally
    public override void SceneLoadLocalDone(string scene)
    {
        //Position to spawn player at
        Vector3 spawnPos = spawnPositions[Random.Range(0, 4)];

        //Spawning the player
        BoltNetwork.Instantiate(BoltPrefabs.Player, spawnPos, Quaternion.identity);
    }
}
