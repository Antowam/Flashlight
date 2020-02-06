using UnityEngine;

public class NetBattery :
Bolt.EntityBehaviour<ICustomPlayerState>
{
    public NetMovement NetworkPlayer;
    SphereCollider sphereCollider;
    public float batteryCharge = 10f;

    const float BATTERY_MAX = 100f;

    private void Awake()
    {
        NetworkPlayer.GetComponentsInChildren<NetMovement>();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("network pickup debug");
        AddBatteryCharge(batteryCharge);
        BoltNetwork.Destroy(gameObject);
    }

    void AddBatteryCharge(float batteryCharge)
    { 
        if(NetworkPlayer.currentBatteryCharge < BATTERY_MAX)
        {
            NetworkPlayer.currentBatteryCharge += Mathf.Clamp(batteryCharge, 0f, BATTERY_MAX);
        }

        
        if (NetworkPlayer.currentBatteryCharge > BATTERY_MAX)
        {
            NetworkPlayer.currentBatteryCharge = BATTERY_MAX;
        }
    }
}
