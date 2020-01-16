using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public float movSpeed;
    public float rotSpeed;
    private Vector3 rotateValue;
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float y = Input.GetAxisRaw("Mouse X");
        float x = Input.GetAxisRaw("Mouse Y");

        rotateValue = new Vector3(x, y * -1, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue;

        if(Input.GetKeyDown(KeyCode.W))
        {
            transform.position += transform.forward * movSpeed;
        }
    }
}
