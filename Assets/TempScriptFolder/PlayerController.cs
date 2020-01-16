using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Private variables
    private float translationZ;
    private float translationX;
    #endregion

    #region Mouse Input
    public const string HorizontalInput = "Horizontal";
    public const string VerticalInput = "Vertical";
    #endregion

    [Header("Player Attributes")]
    public float moveSpeed = 5f;
    public int batterySize = 100;





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
