using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventListener : Bolt.GlobalEventListener
{
    public override void OnEvent(PlayerJoinedEvent evnt)
    {
        gameObject.GetComponent<NetMovement>().EnablePlayerCamera();
        gameObject.GetComponent<NetMovement>().SetPlayerHUD(true);
    }

}
