using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Battery : MonoBehaviour
{
    Flashlight flashLight;
    SphereCollider sphereCollider;
    GameObject player;

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided with player");





        if (other == player)
        {
            //Debug.Log("collided with player");
            //AddBatteryCharge();
        }
    }

    void AddBatteryCharge()
    {
        if(flashLight.currentBattery < flashLight.maxBattery)
        {
            _ = flashLight.currentBattery + charge;
        }
    }
}
