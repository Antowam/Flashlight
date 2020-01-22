using UnityEngine;
using Bolt;

public class NetMouseLook : Bolt.EntityBehaviour<ICustomPlayerState>
{
    //float yaw;
    //float pitch;

    //const float mouseSensitivity = 1.0f;
    //public float clampAngle = 80.0f;

    //public const string MouseXInput = "Mouse X";
    //public const string MouseYInput = "Mouse Y";

    //public GameObject characterBody;

    //private void Start()
    //{
    //    Cursor.visible = false;
    //}

    ////public override void Attached()
    ////{
    ////    state.SetTransforms(state.PlayerTransform, transform);
    ////}

    //void PollKeys()
    //{
    //    yaw += (Input.GetAxisRaw("Mouse X") * mouseSensitivity);
    //    yaw %= 360f;

    //    pitch += (-Input.GetAxisRaw("Mouse Y") * mouseSensitivity);
    //    pitch = Mathf.Clamp(pitch, -85f, +85f);
    //}

    //private void Update()
    //{
    //    //if (entity.HasControl)
    //    //    PollKeys();

    //    //if (entity.IsOwner)
    //    //    Look(yaw, pitch);
    //}

    //public override void SimulateController()
    //{
    //    PollKeys();

    //    ICustomPlayerCommandInput input = CustomPlayerCommand.Create();

    //    input.Yaw = yaw;
    //    input.Pitch = pitch;
        
    //    entity.QueueInput(input);
    //}

    //public override void ExecuteCommand(Command command, bool resetState)
    //{
    //    CustomPlayerCommand cmd = (CustomPlayerCommand)command;

    //    if (resetState)
    //    {
    //        RotateBack(cmd.Result.PlayerRotation);
    //    }
    //    else
    //    {
    //        Look(cmd.Input.Yaw, cmd.Input.Pitch);

    //        cmd.Result.PlayerRotation = characterBody.transform.rotation;
    //    }
    //}

    //public void Look(float yaw, float pitch)
    //{
    //    Camera playerCam = characterBody.GetComponentInChildren<Camera>();
    //    if (playerCam != null)
    //    {
    //        playerCam.transform.rotation = Quaternion.Euler(pitch, 0, 0);
    //    }
    //    gameObject.transform.rotation = Quaternion.Euler(0, yaw, 0);
    //}

    //void RotateBack(Quaternion playerRotation)
    //{
    //    gameObject.transform.localRotation = playerRotation;
    //}
}