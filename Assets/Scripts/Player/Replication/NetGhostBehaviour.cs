using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetGhostBehaviour : Bolt.EntityBehaviour<ICustomPlayerState>
{
    [Header("Behaviors")]
    [SerializeField] private VisibilityTrigger vt;

    void Update(){
        CheckForInput();   
    }

    private void CheckForInput() {
        if (Input.GetKeyDown(KeyCode.V))
            vt.ActivateVisibilityChange();
        if(Input.GetKeyDown(KeyCode.B))
            vt.ActivateDebugTriggerSphere();
    }
}
