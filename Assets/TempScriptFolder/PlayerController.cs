using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public const string HorizontalInput = "Horizontal";
    public const string VerticalInput = "Vertical";
    

    Rigidbody rb;


    public float moveSpeed = 5f;
    public int batterySize = 100;



    private float translationZ;
    private float translationX;







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
