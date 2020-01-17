using UnityEngine;

public class NetFlashlight : Bolt.EntityBehaviour<ICustomPlayerState>
{
    public Light flashLight;
    private bool isLightOn = false;

    public override void Attached()
    {
        flashLight.enabled = false;
        flashLight.intensity = 2f;
        flashLight.spotAngle = 65f;
    }

    public override void SimulateOwner()
    {
        HandleFlashLight(KeyCode.E);
    }

    void HandleFlashLight(KeyCode key)
    {
        if(Input.GetKeyDown(key) && !isLightOn)
        {
            flashLight.enabled = true;
            isLightOn = true;
        }
        else if(Input.GetKeyDown(key) && isLightOn)
        {
            flashLight.enabled = false;
            isLightOn = false;
        }
    }
}
