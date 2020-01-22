using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour]
public class NetworkPlayerCallbacks : Bolt.GlobalEventListener
{
    //public override void SceneLoadLocalDone(string map)
    //{
    //    // this just instantiates our player camera,
    //    // the Instantiate() method is supplied by the BoltSingletonPrefab<T> class
    //    Debug.Log("Scene done loading");
    //    PlayerRenderCamera.Instantiate();
    //}

    //public override void ControlOfEntityGained(BoltEntity entity)
    //{
    //    Debug.Log("YOU NOW HAVE CONTROL");
    //    PlayerRenderCamera.instance.transform.position = entity.transform.position;
    //}
}
