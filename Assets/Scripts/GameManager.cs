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
        StartCoroutine(GameTimer());
    }

    public override void SimulateOwner()
    {
        if(gameStarted && entity.IsOwner)
        {
            gameTimer -= Time.deltaTime;
            state.GameTimer = gameTimer;
        }
    }

    public void AddPlayer(BoltEntity playerObject)
    {
        if(playerObject.CompareTag("Ghost"))
        {
            state.GhostPlayer = playerObject;
            Debug.Log(state.GhostPlayer);
        }
        else if(playerObject.CompareTag("Player"))
        {
            state.PlayerList[state.PlayerIndex] = playerObject;
            Debug.Log(state.PlayerList[state.PlayerIndex]);
            state.PlayerIndex++;
        }
    }

    bool CheckPlayersDead()
    {
        for (int i = 0; i < state.PlayerList.Length; i++)
        {
            if(state.PlayerList[i].gameObject.GetComponentInChildren<NetMovement>().isDead)
            {
                return true;
            }
        }

        //If no players are alive return false
        return false;
    }

    bool CheckGhostDead()
    {
        if(!state.GhostPlayer.GetComponentInChildren<NetMovement>().isDead)
            return false;

        return true;
    }

    public void CheckWhosAlive()
    {
        if(!CheckPlayersDead())
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

    public void CheckGameState()
    {
        switch(gameState)
        {
            case GameState.None:
                break;
            case GameState.PlayersDead:
                Debug.Log("Players Died");
                EnablePlayersHUD("Players Won");
                break;
            case GameState.GhostDead:
                Debug.Log("Ghost Died");
                EnablePlayersHUD("Ghost Won");
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
