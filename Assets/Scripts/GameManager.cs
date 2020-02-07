using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : Bolt.EntityBehaviour<ICustomGameManagerState>
{
    public static GameManager instance;

    public enum GameState
    {
        None,
        PlayersDead,
        GhostDead,
        TimeOut,
    }

    GameState gameState = 0;

    [Header("GameTimer")]
    public bool gameStarted = false;
    float gameTimer = 0.0f;
    public float gameStartTime = 60.0f;

    public bool isGhostDead = false;

    public Dictionary<Transform, bool> spawnPoints = new Dictionary<Transform, bool>();
    public List<BoltConnection> connections = new List<BoltConnection>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameTimer = gameStartTime;
        if(entity.IsOwner)
        {
            state.GameTimer = gameTimer;
        }

        FindSpawnPoints();
    }

    public override void SimulateOwner()
    {
        if(gameStarted && entity.IsOwner)
        {
            gameTimer -= Time.deltaTime;
            state.GameTimer = gameTimer;
        }
    }

    public void OnPlayerJoined(BoltEntity playerObject)
    {
        Debug.LogWarning(playerObject);
        if(playerObject.CompareTag("Ghost"))
        {
            state.GhostPlayer = playerObject;
            isGhostDead = false;
        }
        else if(playerObject.CompareTag("Player"))
        {
            state.PlayerList[state.PlayerIndex] = playerObject;
            state.PlayerIndex++;
        }
    }

    bool ArePlayersDead()
    {
        for (int i = 0; i < state.PlayerList.Length; i++)
        {
            if(state.PlayerList[i] == null)
                continue;

            if(state.PlayerList[i].gameObject.GetComponentInChildren<NetMovement>() != null)
                if(!state.PlayerList[i].gameObject.GetComponentInChildren<NetMovement>().isDead)
                    return false;
        }

        //If no players are alive return true
        return true;
    }

    bool CheckGhostDead()
    {
        return isGhostDead;
    }

    public void CheckWhosAlive()
    {
        if(ArePlayersDead())
        {
            gameState = GameState.PlayersDead;
        }
        if(CheckGhostDead())
        {
            gameState = GameState.GhostDead;
        }

        Debug.Log("Check alive");

        CheckGameState();
    }

    public void KillPlayer(string networkID)
    {
        for (int i = 0; i < state.PlayerList.Length; i++)
        {
            if(state.PlayerList[i] == null)
                continue;

            if(networkID == state.GhostPlayer.NetworkId.ToString())
                isGhostDead = true;
        }

        CheckWhosAlive();
    }

    public void CheckGameState()
    {
        switch(gameState)
        {
            case GameState.None:
                break;
            case GameState.PlayersDead:
                EnablePlayersHUD("Ghost Won");
                break;
            case GameState.GhostDead:
                EnablePlayersHUD("Players Won");
                break;
            case GameState.TimeOut:
                EnablePlayersHUD("Players Won");
                break;
            default:
                break;
        }
    }

    public IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(state.GameTimer);
        gameState = GameState.TimeOut;
        CheckGameState();
    }

    void EnablePlayersHUD(string whoWon)
    {
        for (int i = 0; i < state.PlayerList.Length; i++)
        {
            if (state.PlayerList[i] == null)
                continue;

            if(state.PlayerList[i].gameObject.GetComponentInChildren<NetMovement>() != null)
                state.PlayerList[i].gameObject.GetComponentInChildren<NetMovement>().EnableEndGameHUD(whoWon);
        }
        if (state.GhostPlayer.gameObject.GetComponentInChildren<NetMovement>() != null)
            state.GhostPlayer.gameObject.GetComponentInChildren<NetMovement>().EnableEndGameHUD(whoWon);
    }

    public void EnablePlayerFlashlight(bool isLightOn, string playerID)
    {
        for (int i = 0; i < state.PlayerList.Length; i++)
        {
            if(state.PlayerList[i] == null)
                continue;

            if(!(state.PlayerList[i].NetworkId.ToString() == playerID))
                continue;

            if (state.PlayerList[i].gameObject.GetComponentInChildren<NetMovement>() != null)
                state.PlayerList[i].gameObject.GetComponentInChildren<NetMovement>().HandleFlashLight(isLightOn);
        }
    }

    public void StartRound()
    {
        UpdateConnections();
        ReInitializeSpawnPoints();
        UnassignControl();
        AssignControl();
        //SpawnPlayers();
        //gameStarted = true;
        //StartCoroutine(GameTimer());
    }

    void UnassignControl()
    {
        //Remove control
        for (int i = 0; i < state.PlayerList.Length; i++)
        {
            if(state.PlayerList[i] == null)
                continue;

            if(BoltNetwork.IsServer && state.PlayerList[i].HasControl)
                state.PlayerList[i].ReleaseControl();
            else if(state.PlayerList[i].IsControlled)
                state.PlayerList[i].RevokeControl();
        }

        if(BoltNetwork.IsServer)
            state.GhostPlayer.ReleaseControl();
        else
            state.GhostPlayer.RevokeControl();
    }

    void AssignControl()
    {
        //for (int i = 0; i < connections.Count; i++)
        //{
        //    if (!state.GhostPlayer.IsControlled)
        //    {
        //        int whoToAssign = UnityEngine.Random.Range(0, connections.Count);
        //        if (BoltNetwork.IsServer && entity.IsOwner)
        //            state.GhostPlayer.TakeControl();
        //        else
        //            state.GhostPlayer.AssignControl(connections[whoToAssign]);
        //        connections.RemoveAt(whoToAssign);
        //    }
        //    else if (!state.PlayerList[i].IsControlled)
        //    {
        //        if (BoltNetwork.IsServer && entity.IsOwner)
        //            state.PlayerList[i].TakeControl();
        //        else
        //            state.PlayerList[i].AssignControl(connections[i]);

        //        connections.RemoveAt(i);
        //    }
        //}

        //Assign Control
        int whoToAssign = UnityEngine.Random.Range(0, connections.Count);
        if (BoltNetwork.IsServer)
            state.GhostPlayer.TakeControl();
        else
            state.GhostPlayer.AssignControl(connections[whoToAssign]);
        connections.RemoveAt(whoToAssign);

        for (int i = connections.Count - 1; i >= 0; i--)
        {
            if (state.PlayerList[i] == null)
                continue;

            if (BoltNetwork.IsServer && !state.PlayerList[i].IsControlled)
                state.PlayerList[i].TakeControl();
            else if (BoltNetwork.IsServer && !state.PlayerList[i].IsControlled)
                state.PlayerList[i].AssignControl(connections[i]);

            connections.RemoveAt(i);
        }
    }

    void SpawnPlayers()
    {
        Transform spawnPoint = null;

        foreach(KeyValuePair<Transform, bool> keyPair in spawnPoints)
        {
            if(keyPair.Key == null)
                continue;

            if(keyPair.Value == false)
            {
                spawnPoint = keyPair.Key;
                break;
            }
        }

        SpawnPlayer(spawnPoint);
    }

    void SpawnPlayer(Transform spawnPos)
    {
        for (int i = 0; i < state.PlayerList.Length; i++)
        {
            if(state.PlayerList[i] == null)
                continue;

            //if(!state.PlayerList[i].HasControl)
            //    continue;

            if(state.PlayerList[i].gameObject != null)
            {
                state.PlayerList[i].gameObject.GetComponentInChildren<NetMovement>().gameObject.transform.position = spawnPos.position;
                break;
            }
        }

        //if (state.GhostPlayer.HasControl)
            state.GhostPlayer.gameObject.GetComponentInChildren<NetMovement>().gameObject.transform.position = spawnPos.position;

        spawnPoints[spawnPos] = true;
    }

    void FindSpawnPoints()
    {
        GameObject[] mapSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        for (int i = 0; i < mapSpawnPoints.Length; i++)
            spawnPoints.Add(mapSpawnPoints[i].transform, false);
    }

    void ReInitializeSpawnPoints()
    {
        foreach(Transform transform in spawnPoints.Keys.ToList())
        {
            if (transform == null)
                continue;

            spawnPoints[transform] = false;
        }
    }

    void UpdateConnections()
    {
        foreach (var connection in BoltNetwork.Connections)
        {
            connections.Add(connection);
        }
    }

    public static GameManager GetInstance()
    {
        if(instance == null)
        {
            instance = new GameObject("GameManager").AddComponent<GameManager>();
        }

        return instance;
    }

}
