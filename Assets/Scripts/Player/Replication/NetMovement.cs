using System;
using Bolt;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class NetMovement : Bolt.EntityBehaviour<ICustomPlayerState>
{
    [Header("Player Attributes")]
    private Rigidbody rb;
    GameObject playerBody;
    [SerializeField] float health = 100;
    public bool isDead = false;
    [SerializeField] Slider HealthBar;
    [SerializeField] Slider BatteryBar;

    [Header("Movement Variables")]
    private float translationZ;
    private float translationX;
    float yaw;
    float pitch;
    Vector3 forward;
    Vector3 right;
    public float moveSpeed = 5f;

    [Header("Mouse Settings")]
    const float mouseSensitivity = 1.0f;
    public float clampAngle = 80.0f;

    [Header("Light stuff")]
    public Light flashLightObject;
    public GameObject lightCollider;
    bool isLightOn = false;
    public float currentBatteryCharge = 100f;
    [Tooltip("Drain amount for the flashlight battery")]
    public float drainAmount = 1.0f;

    [Header("GameTime")]
    public float gameTime = 0.0f;
    public TextMeshProUGUI gameTimerText;

    public GameObject endGameHUD;
    public TextMeshProUGUI whoWonText;

    private void Update()
    {
        forward = playerBody.transform.right;
        right = playerBody.transform.forward;

        //Healthbar is for the ghost
        if (HealthBar != null)
            HealthBar.value = health;

        //batterybar is for the players
        if (BatteryBar != null)
            BatteryBar.value = currentBatteryCharge;

        //This was to fix up and down movement of camera but doesnt work smoothly
        //should not getcomponent in update
        //playerCam = playerBody.GetComponentInChildren<Camera>();
        //if (playerCam != null)
        //{
        //    playerCam.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        //}

        gameTime = GameManager.GetInstance().state.GameTimer;
        gameTimerText.text = gameTime.ToString("F2");
    }

    public override void Attached()
    {
        state.SetTransforms(state.PlayerTransform, transform);
        rb = GetComponent<Rigidbody>();
        playerBody = transform.gameObject;

        if(flashLightObject != null && lightCollider != null)
        {
            flashLightObject.enabled = false;
            lightCollider.SetActive(false);
        }
    }

    //This checks for local inputs
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

        if(mouse && !isDead)
        {
            yaw += (Input.GetAxisRaw("Mouse X") * mouseSensitivity);
            yaw %= 360f;

            pitch += (-Input.GetAxisRaw("Mouse Y") * mouseSensitivity);
            pitch = Mathf.Clamp(pitch, -85f, +85f);
        }

        if (!isDead && Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
        {
            isLightOn = !isLightOn;
        }
    }

    //This checks for local inputs on the server
    public override void SimulateController()
    {
        if(entity.HasControl)
            PollKeys(true);

        if (isLightOn && currentBatteryCharge >= 0)
            DrainBattery(drainAmount);

        ICustomPlayerCommandInput input = CustomPlayerCommand.Create();
        
        //send local inputs to server
        input.TranslationX = translationX;
        input.TranslationZ = translationZ;
        input.Yaw = yaw;
        input.Pitch = pitch;
        input.isFlashlightOn = isLightOn;

        entity.QueueInput(input);
    }

    public override void ExecuteCommand(Command command, bool resetState)
    {
        CustomPlayerCommand cmd = (CustomPlayerCommand)command;

        if(resetState)
        {
            //if player is non synced send him back to old location
            GoBack(cmd.Result.PlayerPosition, cmd.Result.PlayerRotation);
            HandleFlashLight(cmd.Result.isFlashlightOn);
        }
        else
        {
            //send move from server to player
            Move(cmd.Input.TranslationX, cmd.Input.TranslationZ, cmd.Input.PlayerPosition, cmd.Input.Yaw, cmd.Input.Pitch);
            HandleFlashLight(cmd.Input.isFlashlightOn);

            //set synced variables to player variables
            cmd.Result.PlayerPosition = gameObject.transform.position;
            cmd.Result.PlayerRotation = gameObject.transform.rotation;
            cmd.Result.isFlashlightOn = isLightOn;
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
        //Set local isdead to true //This works
        isDead = true;

        var dieEvent = PlayerDiedEvent.Create();
        dieEvent.IsDead = isDead;
        dieEvent.PlayerWhoDied = entity.NetworkId.ToString();
        dieEvent.Send();

        //Stop the players movement
        rb.velocity = Vector3.zero;
    }

    void Revived()
    {
        if (isDead == true)
        {
            isDead = false;
            var dieEvent = PlayerDiedEvent.Create();
            dieEvent.IsDead = isDead;
            dieEvent.PlayerWhoDied = entity.NetworkId.ToString();
            dieEvent.Send();
        }
    }

    void HandleFlashLight(bool isOn)
    {
        if(flashLightObject != null && lightCollider != null)
        {
            if(isOn)
            {
                flashLightObject.enabled = true;
                lightCollider.SetActive(true);
            }
            else
            {
                flashLightObject.enabled = false;
                lightCollider.SetActive(false);
            }
        }
    }

    void DrainBattery(float drainValue)
    {
        currentBatteryCharge -= drainValue * Time.deltaTime;
        if (currentBatteryCharge <= 0)
        {
            HandleFlashLight(false);
        }
    }

    void FlashLightCollisionEffect()
    {
        //what happens if a ghost enters the 
        if(gameObject.CompareTag("Ghost"))
        {
            health -= Time.deltaTime * 2.0f;
            if(health <= 0)
            {
                Die();
            }
        }
    }

    public void EnableEndGameHUD(string whoWon)
    {
        whoWonText.text = whoWon;
        endGameHUD.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != this.gameObject)
        {
            if(gameObject.CompareTag("Ghost"))
            {
                if(other.CompareTag("Player"))
                {
                    NetMovement playerMove = other.GetComponent<NetMovement>();
                    if (playerMove != null)
                    {
                        playerMove.Die();
                    }
                }
            }
            else if(gameObject.CompareTag("Player"))
            {
                if (other.CompareTag("Player"))
                {
                    NetMovement playerMove = other.GetComponent<NetMovement>();
                    if (playerMove != null)
                    {
                        playerMove.Revived();
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject != this.gameObject)
        {
            if(gameObject.CompareTag("Ghost"))
            {
                if(other.gameObject.CompareTag("Flashlight"))
                {
                    FlashLightCollisionEffect();
                }
            }
        }
    }
}

