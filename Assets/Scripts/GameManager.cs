using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour]
public class GameManager : Bolt.GlobalEventListener
{
    public static GameManager instance;

    public List<NetPlayerHandling> players = new List<NetPlayerHandling>();

    public GameObject[] playerObjects = new GameObject[4];

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //GetInstance().GetPlayers();
    }

    public void GetPlayers()
    {
        players = NetworkPlayerRegistry.GetAllPlayers;

        for (int i = 0; i < players.Count; i++)
        {
            if(players[i].character.gameObject != null)
                playerObjects[i] = players[i].character.gameObject;
        }
    }

    void RespawnPlayers()
    {

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
