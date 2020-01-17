using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetGhostBehaviour : Bolt.EntityBehaviour<ICustomPlayerState>
{
    [SerializeField] private GameObject _mesh;

    public override void Attached()
    {
        _mesh.SetActive(false);
    }

    public override void SimulateOwner()
    {

    }
}
