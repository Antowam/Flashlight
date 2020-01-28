using UnityEngine;
using Bolt;

public class NetFlashlight : Bolt.EntityBehaviour<ICustomPlayerState>
{
    //public Light flashLight;
    //private bool isLightOn;
    //public int batterySize = 100;

    //public override void Attached()
    //{
    //    flashLight.enabled = false;
    //    flashLight.intensity = 2f;
    //    flashLight.spotAngle = 65f;
    //}

    //void PollKeys()
    //{
    //    if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
    //    {
    //        isLightOn = !isLightOn;
    //    }
    //}

    //public override void SimulateController()
    //{
    //    PollKeys();

    //    IFlashLightCommandsInput flashlighInput = FlashLightCommands.Create();

    //    //send local inputs to server
    //    flashlighInput.isFlashlightOn = isLightOn;

    //    entity.QueueInput(flashlighInput);
    //}

    //public override void ExecuteCommand(Command command, bool resetState)
    //{
    //    FlashLightCommands cmd = (FlashLightCommands)command;

    //    if (resetState)
    //    {
    //        //if player is non synced send him back to old location
    //        HandleFlashLight(cmd.Result.isFlashlightOn);
    //    }
    //    else
    //    {
    //        HandleFlashLight(cmd.Input.isFlashlightOn);
    //        cmd.Result.isFlashlightOn = isLightOn;
    //    }
    //}

    //public void HandleFlashLight(bool isOn)
    //{
    //    if(isOn)
    //    {
    //        flashLight.enabled = true;
    //    }
    //    else
    //    {
    //        flashLight.enabled = false;
    //    }
    //}

}
