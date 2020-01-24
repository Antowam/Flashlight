using UnityEngine;

public class NetBattery :
Bolt.EntityBehaviour<ICustomPlayerState>
{
    public GameObject player;
    public Flashlight flashLight;
    SphereCollider sphereCollider;
    public float batteryCharge = 10f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        flashLight = player.GetComponent<Flashlight>();

        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("asdasdasd");
    }
}
