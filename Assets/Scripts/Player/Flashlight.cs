﻿using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashLight;
    private bool isLightOn = false;

    private void Awake()
    {
        flashLight.enabled = false;
        flashLight.intensity = 2f;
        flashLight.spotAngle = 65f;
    }

    private void Update()
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
