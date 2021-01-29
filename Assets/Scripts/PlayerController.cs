using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range (0f,100f)]
    private float playerSpeed = .1f;

    [SerializeField, Range(0f, 10f)]
    private float sensitivityX = 1f;

    [SerializeField, Range(0f, 10f)]
    private float sensitivityY = 1f;

    [SerializeField, Tooltip("x component is Min & y component is Max")]
    private Vector2 clampAnglesY = new Vector2(0, 0);

    [Header("References")]
    [SerializeField]
    private GrabScript grabScript;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private Transform grabPoint;

    private Vector3 playerVelocity = Vector3.zero;
    private Vector2 rotation = new Vector2(0, 90);
    private Rigidbody playerRigidbody;


    void Awake()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateMouseMovement(cameraTransform);


        if (Input.GetMouseButtonDown(0))
        {
            grabScript.SelectItem();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            grabScript.DeselectItem();
        }


        // Unlock and show cursor when right mouse button released
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKey(KeyCode.W)) 
        {
            playerVelocity.x = 1;
        } else if (Input.GetKey(KeyCode.S))
        {
            playerVelocity.x = -1;
        } else
        {
            playerVelocity.x = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerVelocity.z = -1;
        } else if (Input.GetKey(KeyCode.D))
        {
            playerVelocity.z = 1;
        }
        else
        {
            playerVelocity.z = 0;
        }
    }

    void FixedUpdate()
    {
        UpdateCharacterRotation(cameraTransform.eulerAngles.y);
        //playerRigidbody.velocity += (playerVelocity * Time.deltaTime * playerSpeed);
        //playerRigidbody.AddRelativeForce(new Vector3(playerVelocity.x, 0, playerVelocity.z) * playerSpeed);
        playerRigidbody.AddForce((transform.forward * playerSpeed * playerVelocity.x) + (transform.right * playerSpeed * playerVelocity.z));
        //playerRigidbody.velocity = (transform.forward * Time.fixedDeltaTime * playerSpeed * playerVelocity.x) + (transform.right * Time.fixedDeltaTime * playerSpeed * playerVelocity.z);
    }

    void SetPlayerVelocity(string directionName, int directionScalar)
    {
        if (directionName == "x")
        {
            playerVelocity.x = directionScalar;
        }
        
        if (directionName == "z")
        {
            playerVelocity.z = directionScalar;
        }
    }

    void UpdateMouseMovement(Transform mouseTransform)
    {

        rotation.y += Input.GetAxis("Mouse X") * sensitivityY;
        rotation.x += -Input.GetAxis("Mouse Y") * sensitivityX;

        // Clamp mouse between max values to prevent camera issues
        rotation.x = Mathf.Clamp(rotation.x, clampAnglesY.x, clampAnglesY.y);
        //mouseTransform.eulerAngles = new Vector2(ClampAngle(-rotation.x, ClampAnglesY.x, ClampAnglesY.y), rotation.y);
        mouseTransform.eulerAngles = new Vector2(rotation.x, rotation.y);
    }

    // only update y axis
    void UpdateCharacterRotation(float currentRotationY)
    {
        playerRigidbody.transform.eulerAngles = new Vector2(playerRigidbody.transform.eulerAngles.x, currentRotationY);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(grabPoint.position, .1f);
    }
}
