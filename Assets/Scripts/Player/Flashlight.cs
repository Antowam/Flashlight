using UnityEngine;


//Script that handles and control the flashlight
public class Flashlight : MonoBehaviour
{
    PlayerController controller;
    FlashlightFlickerEffect flickerEffect;
    public Light flashLight;

    private bool isLightOn = false;
    public float maxBattery = 100f;
    [HideInInspector]public float currentBattery;
    public bool flicker;

    [Tooltip("Drain amount for the flashlight battery")]
    public float drainAmount;




    private void Awake()
    {
        flickerEffect = GetComponent<FlashlightFlickerEffect>();
        SetFlicker();

        currentBattery = maxBattery;
        flashLight.enabled = false;
        flashLight.intensity = 2f;
        flashLight.spotAngle = 65f;
        flashLight.range = 10f;
    }

    private void Update()
    {
        if (isLightOn)
        {
            DrainBattery(drainAmount);
        }

        HandleFlashLight(KeyCode.E);
        
    }

    void HandleFlashLight(KeyCode key)
    {
        if(Input.GetKeyDown(KeyCode.Q) && isLightOn)
        {
            DetectEnemy();
        }

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
        if(currentBattery <= 0)
        {
            isLightOn = false;
        }
    }

    void SetFlicker()
    {
        if (flicker)
        {
            flickerEffect.enabled = true;
        }
        else
        {
            flickerEffect.enabled = false;
        }
    }

    void DetectEnemy()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, flashLight.range))
        {
            Debug.DrawRay(transform.position, Vector3.forward, Color.red);
            Debug.Log("Did Hit " + hit.collider);
        }
    }
}
