using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJoined : Bolt.EntityBehaviour<ICustomPlayerState>
{
    public Camera entityCamera;
    public GameObject playerHUD;
    public Slider batteryChargeSlider;
    private string username;

    /*
    public override void Attached() {
        if(entity.IsOwner) {
            entityCamera.gameObject.SetActive(true);
        }
    }*/

    //public override void SimulateController()
    //{
    //    if (entity.HasControl && entityCamera.gameObject.activeInHierarchy == false)
    //    {
    //        entityCamera.gameObject.SetActive(true);
    //    }
    //}

    //public void ActivateCamera(BoltConnection connection)
    //{
    //    if (entity.IsController(connection) && entityCamera.gameObject.activeInHierarchy == false)
    //    {
    //        entityCamera.gameObject.SetActive(true);
    //    }
    //    else if(entity.IsOwner)
    //    {
    //        entityCamera.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        entityCamera.gameObject.SetActive(false);
    //    }
    //}

    private void Update()
    {
        if (entity.HasControl)
        {
            if(entityCamera.gameObject.activeInHierarchy == false)
            {
                entityCamera.gameObject.SetActive(true);
            }
            if(playerHUD != null)
            {
                playerHUD.gameObject.SetActive(true);
                if(batteryChargeSlider == null)
                {
                    batteryChargeSlider = GetComponentInChildren<Slider>();
                }
            }
        }
    }

    //private void Update()
    //{
    //    if (entity.IsControllerOrOwner && entityCamera.gameObject.activeInHierarchy == false)
    //    {
    //        entityCamera.gameObject.SetActive(true);
    //    }
    //}
}

