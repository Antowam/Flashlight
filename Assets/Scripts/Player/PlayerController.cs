using System;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
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
    
    
    private void FixedUpdate()
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

