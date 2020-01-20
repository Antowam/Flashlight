using UnityEngine;

public class NetMouseLook : Bolt.EntityBehaviour<ICustomPlayerState>
{
    Vector3 _mouseAbsolute;
    Vector3 _smoothMouse;

    public const string MouseXInput = "Mouse X";
    public const string MouseYInput = "Mouse Y";

    public Vector2 clampInDegrees = new Vector2(360, 180);
    public bool lockCursor;
    public Vector3 sensitivity = new Vector3(2, 2, 2);
    public Vector2 smoothing = new Vector3(3, 3, 3);
    public Vector3 targetDirection;
    public Vector3 targetCharacterDirection;
    public GameObject characterBody;

    public override void Attached()
    {
        Cursor.visible = false;

        targetDirection = transform.localRotation.eulerAngles;

        if (characterBody)
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
    }

    public override void SimulateOwner()
    {
        Look();
    }

    public void Look()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (lockCursor)
                lockCursor = false;
            else if (!lockCursor)
                lockCursor = true;
        }

        if (lockCursor)
            return;

        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        var mouseDelta = new Vector2(Input.GetAxisRaw(MouseXInput), Input.GetAxisRaw(MouseYInput));

        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        _mouseAbsolute += _smoothMouse;

        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        if (characterBody)
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
            characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }
        else
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }
    }
} 