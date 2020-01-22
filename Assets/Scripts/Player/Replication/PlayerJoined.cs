﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoined : Bolt.EntityBehaviour<ICustomPlayerState>
{
    public Camera entityCamera;
    private string username;

    private void Update() {
        if (entity.IsOwner && entityCamera.gameObject.activeInHierarchy == false) {
            entityCamera.gameObject.SetActive(true);
        }
    }
}