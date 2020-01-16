using UnityEngine;


[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody rb;


    public float speed = 1f;



    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        
    }
}
