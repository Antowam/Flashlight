using System;
using Bolt;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class NetMovement : Bolt.EntityBehaviour<ICustomPlayerState>
{
    private float translationZ;
    private float translationX;

    float yaw;
    float pitch;

    const float mouseSensitivity = 1.0f;
    public float clampAngle = 80.0f;

    public bool isDead = false;
    
    private Rigidbody rb;
    GameObject playerBody;
    PlayerJoined playerJoined;
    Vector3 forward;
    Vector3 right;

    [Header("Player Attributes")]
    public float moveSpeed = 5f;
    public int batterySize = 100;

    public override void Attached()
    {
        state.SetTransforms(state.PlayerTransform, transform);
        rb = GetComponent<Rigidbody>();
        playerJoined = GetComponent<PlayerJoined>();
        playerBody = transform.gameObject;
    }

    void PollKeys(bool mouse)
    {
        if(!isDead)
        {
            translationX = Input.GetAxisRaw("Horizontal");
            translationZ = Input.GetAxisRaw("Vertical");
        }
        else
        {
            translationX = 0;
            translationZ = 0;
        }

        if(mouse)
        {
            yaw += (Input.GetAxisRaw("Mouse X") * mouseSensitivity);
            yaw %= 360f;

            pitch += (-Input.GetAxisRaw("Mouse Y") * mouseSensitivity);
            pitch = Mathf.Clamp(pitch, -85f, +85f);
        }
    }

    private void Update()
    {
        if(entity.HasControl)
            PollKeys(true);

        forward = playerBody.transform.right;
        right = playerBody.transform.forward;

        if(playerJoined.batteryChargeSlider != null)
            playerJoined.batteryChargeSlider.value = batterySize;

        //This was to fix up and down movement of camera but doesnt work smoothly
        //should not getcomponent in update
        //playerCam = playerBody.GetComponentInChildren<Camera>();
        //if (playerCam != null)
        //{
        //    playerCam.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        //}
    }

    public override void SimulateController()
    {
        PollKeys(false);

        ICustomPlayerCommandInput input = CustomPlayerCommand.Create();
        
        //send local inputs to server
        input.TranslationX = translationX;
        input.TranslationZ = translationZ;
        input.Yaw = yaw;
        input.Pitch = pitch;

        entity.QueueInput(input);
    }

    public override void ExecuteCommand(Command command, bool resetState)
    {
        CustomPlayerCommand cmd = (CustomPlayerCommand)command;

        if(resetState)
        {
            //if player is non synced send him back to old location
            GoBack(cmd.Result.PlayerPosition, cmd.Result.PlayerRotation);
        }
        else
        {
            //send move from server to player
            Move(cmd.Input.TranslationX, cmd.Input.TranslationZ, cmd.Input.PlayerPosition, cmd.Input.Yaw, cmd.Input.Pitch);

            //set synced variables to player variables
            cmd.Result.PlayerPosition = gameObject.transform.position;
            cmd.Result.PlayerRotation = gameObject.transform.rotation;
        }
    }

    private void Move(float newTranslationX, float newTranslationZ, Vector3 playerPosition, float yaw, float pitch)
    {
        //Calculate player movement
        forward.y = 0.0f;
        right.y = 0.0f;
        forward.Normalize();
        right.Normalize();

        //calculate direction for movement
        Vector3 desiredMoveDir = (forward * newTranslationX) + (right * newTranslationZ);
        desiredMoveDir.Normalize();

        //Calculate player rotation with mouse
        Quaternion playerRotation = Quaternion.Euler(0, yaw, 0);

        transform.position += (desiredMoveDir * moveSpeed * Time.deltaTime);
        transform.rotation = playerRotation;
    }

    void GoBack(Vector3 playerPosition, Quaternion playerRotation)
    {
        //set player position to old position
        gameObject.transform.position = playerPosition;
        //set player rotation to old rotation
        gameObject.transform.rotation = playerRotation;

        //Move back the player
        Move(0, 0, playerPosition - gameObject.transform.localPosition, playerRotation.y - gameObject.transform.localRotation.y, playerRotation.x - gameObject.transform.localRotation.x);
    }

    void Die()
    {
        isDead = true;
        if (isDead == true)
        {
            Debug.Log("YOU DIED, do cool stuff here");
            //Stop the players movement
            rb.velocity = Vector3.zero;
        }
    }

    void Revived()
    {
        if(isDead == true)
        {
            isDead = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != this.gameObject)
        {
            if(other.CompareTag("Ghost"))
            {
                Die();
            }
            else if(other.CompareTag("Player"))
            {
                Revived();
            }
        }
    }
}

