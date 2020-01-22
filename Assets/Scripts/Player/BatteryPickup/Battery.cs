using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Battery : MonoBehaviour
{
    GameObject player;
    Flashlight flashLight;
    SphereCollider sphereCollider;

    public float charge;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
    }

    private void Update()
    {
        
    }
}
