using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour]
public class ServerEventListener : Bolt.GlobalEventListener
{
    public override void OnEvent(PlayerDiedEvent evnt)
    {
        Debug.LogWarning("TrIED TO KILL pLaYer");
        GameManager.GetInstance().KillPlayer(evnt.PlayerWhoDied);
    }

    public override void OnEvent(PlayerJoinedEvent evnt)
    {
        Debug.LogWarning("player joined the game idiot");
        GameManager.GetInstance().OnPlayerJoined(evnt.PlayerThatJoined);
    }
}
