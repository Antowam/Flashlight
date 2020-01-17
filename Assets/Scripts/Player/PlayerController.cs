using System;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";

    private float translationZ;
    private float translationX;

    Rigidbody rb;

    [Header("Player Attributes")]
    public float moveSpeed = 5f;


    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        translationZ = Input.GetAxis(Vertical) * moveSpeed * Time.deltaTime;
        translationX = Input.GetAxis(Horizontal) * moveSpeed * Time.deltaTime;
        transform.Translate(translationX, 0, translationZ);
    }
}

