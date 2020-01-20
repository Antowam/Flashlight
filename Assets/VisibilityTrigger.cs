using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityTrigger : Bolt.EntityBehaviour<ICustomPlayerState>
{
    [SerializeField] private GameObject _mesh;
    private SphereCollider sc;

    public override void Attached() {
        sc = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
            _mesh.SetActive(true);
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player")
            _mesh.SetActive(false);
    }
}
