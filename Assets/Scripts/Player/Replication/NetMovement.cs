using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NetMovement : Bolt.EntityBehaviour<ICustomPlayerState>
{
    private float translationZ;
    private float translationX;

    private Rigidbody rb;

    [Header("Player Attributes")]
    public float moveSpeed = 5f;
    public int batterySize = 100;

    public override void Attached()
    {
        state.SetTransforms(state.PlayerTransform, transform);
        rb = GetComponent<Rigidbody>();
    }

    public override void SimulateOwner()
    {
        Move();
    }

    private void Move()
    {
        translationZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        translationX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.Translate(translationX, 0, translationZ);
    }
}

