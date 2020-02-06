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

    public override void OnEvent(OnFlashlightActivate evnt)
    {
        Debug.LogWarning("DID YOU ACTIVATE FLASHLIGHT");
        if (evnt.whoActivatedFlashlight == gameObject.GetComponent<NetMovement>().entity.NetworkId.ToString())
            gameObject.GetComponent<NetMovement>().HandleFlashLight(evnt.isFlashlightOn);
    }

}
