using BWolf.Utilities.AudioPlaying;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField, Range(0f, 100f)]
    private float playerSpeed = .1f;

    [SerializeField, Range(0f, 20f)]
    private float throwingForce = 10f;

    [Header("Camera Settings")]
    [SerializeField, Range(0f, 10f)]
    private float sensitivityX = 1f;

    [SerializeField, Range(0f, 10f)]
    private float sensitivityY = 1f;

    [SerializeField, Tooltip("x component is Min & y component is Max")]
    private Vector2 clampAnglesY = new Vector2(0, 0);

    [Header("Sound")]
    [SerializeField]
    private AudioCueSO audioCue = null;

    [SerializeField]
    private AudioConfigurationSO config = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private AudioRequestChannelSO channel = null;

    [Header("References")]
    [SerializeField]
    private GrabScript grabScriptLeftHand;

    [SerializeField]
    private GrabScript grabScriptRightHand;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private Transform grabPointLeft;

    [SerializeField]
    private Transform grabPointRight;

    [SerializeField]
    private GameObject crosshair;

    [Header("Project References")]
    [SerializeField]
    private GameFlowSettings gameFlow = null;

    [SerializeField]
    private SharedPlayerData sharedData = null;

    [SerializeField]
    private BoolChannel pauseChannel = null;

    private Vector3 playerVelocity = Vector3.zero;

    private Rigidbody playerRigidbody;

    private bool pressW = false;
    private bool pressA = false;
    private bool pressS = false;
    private bool pressD = false;

    private int walkTimer;

    private Vector3 lastPos;
    private float walkSpeed = 0;

    private Vector3 localCameraEulerAngles;

    private Vector3 startLocalCameraEulerAngles;
    private Vector3 startWorldPosition;
    private Vector3 startLocalEulerAngles;
    private bool _canGrab = true;

    public GrabScript LeftHand => grabScriptLeftHand;
    public GrabScript RightHand => grabScriptRightHand;

    public bool CanGrab
    {
        get
        {
            return _canGrab;
        }
        set
        {
            _canGrab = value;
            crosshair.SetActive(_canGrab);
        }
    }

    public bool HasCameraAttachedToHead { get; private set; }

    private void Awake()
    {
        gameFlow.OnGameStateChanged += OnGameStateChanged;

        SetGameCursorActiveState(false);
        pauseChannel.OnEventRaised += OnGamePause;

        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        playerSpeed *= playerRigidbody.mass / 2;

        localCameraEulerAngles = cameraTransform.localEulerAngles;

        startLocalCameraEulerAngles = localCameraEulerAngles;
        startWorldPosition = transform.position;
        startLocalEulerAngles = transform.localEulerAngles;

        sharedData.controller = this;
    }

    private void OnEnable()
    {
        playerRigidbody.isKinematic = false;
    }

    private void OnDisable()
    {
        playerRigidbody.isKinematic = true;
    }

    private void Start()
    {
        if (gameFlow.StartAtComputer)
        {
            Enable(false);
        }
    }

    private void OnGameStateChanged(GameStateChange gameStateChange)
    {
        switch (gameStateChange)
        {
            case GameStateChange.TutorialStarted:
                OnTutorialStart();
                break;

            case GameStateChange.GameStarted:
                OnGameStart();
                break;

            case GameStateChange.OnGameEnd:
                OnGameEnd();
                break;

            default:
                break;
        }
    }

    public bool IsInGrabRange(Vector3 positionOfObject)
    {
        return RightHand.IsInGrabRange(positionOfObject) || LeftHand.IsInGrabRange(positionOfObject);
    }

    public void SetCameraAttachedToHead(bool value)
    {
        HasCameraAttachedToHead = value;
    }

    private void SetGameCursorActiveState(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }

    private void OnGameEnd()
    {
        Enable(false);

        transform.position = startWorldPosition;
        transform.localEulerAngles = startLocalEulerAngles;
        cameraTransform.localEulerAngles = startLocalCameraEulerAngles;

        localCameraEulerAngles = cameraTransform.localEulerAngles;
    }

    private void OnGamePause(bool value)
    {
        SetGameCursorActiveState(value);
        Enable(!value);
    }

    private void OnTutorialStart()
    {
        Enable(true);
    }

    private void OnGameStart()
    {
        Enable(true);

        if (!_canGrab)
        {
            CanGrab = true;
        }
    }

    private void Enable(bool value)
    {
        enabled = value;
    }

    private void Update()
    {
        if (!HasCameraAttachedToHead)
        {
            return;
        }

        UpdateMouseMovement(cameraTransform);

        if (Input.GetMouseButtonDown(0) && CanGrab)
        {
            grabScriptLeftHand.SelectItem();
        }

        if (Input.GetMouseButtonUp(0))
        {
            grabScriptLeftHand.DeselectItem();
        }

        if (Input.GetMouseButtonDown(1) && CanGrab)
        {
            grabScriptRightHand.SelectItem();
        }

        if (Input.GetMouseButtonUp(1))
        {
            grabScriptRightHand.DeselectItem();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            grabScriptLeftHand.ThrowItemsAway(throwingForce);
            grabScriptRightHand.ThrowItemsAway(throwingForce);
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (!pressW || !pressS)
            {
                playerVelocity.x = 1;
            }
            pressW = true;
        }
        else
        {
            pressW = false;
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (!pressS || !pressW)
            {
                playerVelocity.x = -1;
            }
            pressS = true;
        }
        else
        {
            pressS = false;
        }

        if (!(pressW ^ pressS))
        {
            playerVelocity.x = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (!pressA || !pressD)
            {
                playerVelocity.z = -1;
            }
            pressA = true;
        }
        else
        {
            pressA = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (!pressD || !pressA)
            {
                playerVelocity.z = 1;
            }
            pressD = true;
        }
        else
        {
            pressD = false;
        }

        if (!(!pressA ^ !pressD))
        {
            playerVelocity.z = 0;
        }
    }

    private void FixedUpdate()
    {
        UpdateCharacterRotation(cameraTransform.eulerAngles.y);
        //playerRigidbody.velocity += (playerVelocity * Time.deltaTime * playerSpeed);
        //playerRigidbody.AddRelativeForce(new Vector3(playerVelocity.x, 0, playerVelocity.z) * playerSpeed);
        Vector3 newVelocity = playerVelocity.normalized;
        playerRigidbody.AddForce((transform.forward * playerSpeed * newVelocity.x) + (transform.right * playerSpeed * newVelocity.z));
        //playerRigidbody.velocity = (transform.forward * Time.fixedDeltaTime * playerSpeed * playerVelocity.x) + (transform.right * Time.fixedDeltaTime * playerSpeed * playerVelocity.z);

        float playerVel = Mathf.Abs(playerVelocity.x) + Mathf.Abs(playerVelocity.z);

        float dPosY = Mathf.Abs(lastPos.y - transform.position.y);
        float dPosX = Mathf.Abs(lastPos.x - transform.position.x);
        float dPosZ = Mathf.Abs(lastPos.z - transform.position.z);

        walkSpeed = dPosX + dPosZ;

        lastPos = transform.position;

        if (walkSpeed > 0.05f)
        {
            walkTimer++;

            if (walkTimer >= 18)
            {
                float tempVol = config.volume;
                config.volume = tempVol / 2 * (Mathf.Min(walkSpeed, 2) * 10);

                channel.RaiseEvent(config, audioCue, transform.position - Vector3.down * 1.5f);

                config.volume = tempVol;

                walkTimer = 0;
            }
        }
        else
        {
            walkTimer = 14;
        }
    }

    private void UpdateMouseMovement(Transform mouseTransform)
    {
        localCameraEulerAngles.y += Input.GetAxis("Mouse X") * sensitivityY;
        localCameraEulerAngles.x += -Input.GetAxis("Mouse Y") * sensitivityX;

        // Clamp mouse between max values to prevent camera issues
        localCameraEulerAngles.x = Mathf.Clamp(localCameraEulerAngles.x, clampAnglesY.x, clampAnglesY.y);
        //mouseTransform.eulerAngles = new Vector2(ClampAngle(-rotation.x, ClampAnglesY.x, ClampAnglesY.y), rotation.y);
        mouseTransform.eulerAngles = new Vector2(localCameraEulerAngles.x, localCameraEulerAngles.y);
    }

    // only update y axis
    private void UpdateCharacterRotation(float currentRotationY)
    {
        playerRigidbody.transform.eulerAngles = new Vector2(playerRigidbody.transform.eulerAngles.x, currentRotationY);
    }

    private void OnDrawGizmos()
    {
        if (!grabPointLeft || !grabPointRight) return;

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(grabPointRight.position, .03f);
        Gizmos.DrawSphere(grabPointLeft.position, .03f);
    }
}