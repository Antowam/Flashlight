using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour]
public class ServerEventListener : Bolt.GlobalEventListener
{
    public override void OnEvent(PlayerDiedEvent evnt)
    {
        GameManager.GetInstance().KillPlayer(evnt.PlayerWhoDied);
    }

    public override void OnEvent(PlayerJoinedEvent evnt)
    {
        GameManager.GetInstance().OnPlayerJoined(evnt.PlayerThatJoined);
    }
}
