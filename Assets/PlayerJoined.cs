using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoined : Bolt.EntityBehaviour<ICustomPlayerState>
{
    public Camera entityCamera;
    private string username;

    /*
    public override void Attached() {
        if(entity.IsOwner) {
            entityCamera.gameObject.SetActive(true);
        }
    }*/

    private void Update() {
        if (entity.IsOwner && entityCamera.gameObject.activeInHierarchy == false) {
            entityCamera.gameObject.SetActive(true);
        }
    }
}
