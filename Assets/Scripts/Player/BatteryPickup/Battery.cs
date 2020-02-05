using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Battery : MonoBehaviour
{
    public Flashlight Player;
    public NetMovement netPlayer;
    SphereCollider sphereCollider;
    public float batteryCharge = 10f;

    private void Awake()
    {
        Player = FindObjectOfType<Flashlight>();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        AddBatteryCharge(batteryCharge);
        Destroy(gameObject);
    }

    void AddBatteryCharge(float batteryCharge)
    {
        if(Player.currentBattery < Player.maxBattery)
        {
            Player.currentBattery += Mathf.Clamp(batteryCharge, 0f, Player.maxBattery);
        }

        if(Player.currentBattery > Player.maxBattery)
        {
            Player.currentBattery = Player.maxBattery;
        }
    }
}
