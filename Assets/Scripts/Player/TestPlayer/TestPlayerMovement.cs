using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TestPlayerMovement : Bolt.EntityBehaviour<ICustomPlayerState>
{
    private float translationZ;
    private float translationX;

    private Rigidbody rb;

    [Header("Player Attributes")]
    public float moveSpeed = 5f;
    public int batterySize = 100;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Networked void start
    public override void Attached()
    {
        state.SetTransforms(state.PlayerTransform, gameObject.transform);
    }

    //private void FixedUpdate()
    //{
    //    Move();
    //}

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

