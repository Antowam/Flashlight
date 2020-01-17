using UnityEngine;


//Script that handles and control the flashlight
public class Flashlight : MonoBehaviour
{
    PlayerController controller;
    FlashlightFlickerEffect flickerEffect;

    public Light flashLight;

    private bool isLightOn = false;
    public float maxBattery = 100f;
    private float currentBattery;




    private void Awake()
    {
        flickerEffect = GetComponent<FlashlightFlickerEffect>();
        flickerEffect.enabled = false;

        currentBattery = maxBattery;
        flashLight.enabled = false;
        flashLight.intensity = 2f;
        flashLight.spotAngle = 65f;
    }

    private void Update()
    {
        if (isLightOn)
        {
            DrainBattery(4f);
            Debug.Log("Battery: " + currentBattery);
        }
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

    void DrainBattery(float drainValue)
    {
        currentBattery -= drainValue * Time.deltaTime;
        if (currentBattery < 35)
        {
            flickerEffect.enabled = true;
        }
    }
}
