using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    bool gameStarted = true;
    float gameTimer = 0.0f;
    public float gameStartTime = 60.0f;

    public List<bool> deadPlayerList = new List<bool>();
    public bool isGhostDead = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameTimer = gameStartTime;
        if(entity.IsOwner)
        {
            for (int i = 0; i < state.PlayerList.Length; i++)
            {
                state.PlayerList[i] = null;
            }

            state.GameTimer = gameTimer;
        }
        //StartCoroutine(GameTimer());
    }

    public override void SimulateOwner()
    {
        if(gameStarted && entity.IsOwner)
        {
            //gameTimer -= Time.deltaTime;
            //state.GameTimer = gameTimer;
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
            deadPlayerList.Add(false);
        }
    }

    bool ArePlayersDead()
    {
        for (int i = 0; i < deadPlayerList.Count; i++)
        {
            if(deadPlayerList[i] == false)
            {
                return false;
            }
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
        for (int i = 0; i < deadPlayerList.Count; i++)
        {
            if(networkID == state.PlayerList[i].NetworkId.ToString())
            {
                deadPlayerList[i] = true;
            }
            else if(networkID == state.GhostPlayer.NetworkId.ToString())
            {
                isGhostDead = true;
            }
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
                Debug.LogWarning("Players Died");
                EnablePlayersHUD("Ghost Won");
                break;
            case GameState.GhostDead:
                Debug.LogWarning("Ghost Died");
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
            {
                state.PlayerList[i].gameObject.GetComponentInChildren<NetMovement>().EnableEndGameHUD(whoWon);
            }
        }
        if (state.GhostPlayer.gameObject.GetComponentInChildren<NetMovement>() != null)
        {
            state.GhostPlayer.gameObject.GetComponentInChildren<NetMovement>().EnableEndGameHUD(whoWon);
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
