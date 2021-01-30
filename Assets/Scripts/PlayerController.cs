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
    private GrabScript grabScriptLeftHand;

    [SerializeField]
    private GrabScript grabScriptRightHand;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private Transform grabPointLeft;

    [SerializeField]
    private Transform grabPointRight;

    private Vector3 playerVelocity = Vector3.zero;
    private Vector2 rotation = new Vector2(0, 90);
    private Rigidbody playerRigidbody;


    void Awake()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        UpdateMouseMovement(cameraTransform);

        if (Input.GetMouseButtonDown(0))
        {
            grabScriptLeftHand.SelectItem();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            grabScriptLeftHand.DeselectItem();
        }

        if (Input.GetMouseButtonDown(1))
        {
            grabScriptRightHand.SelectItem();
        }

        if (Input.GetMouseButtonUp(1))
        {
            grabScriptRightHand.DeselectItem();
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    grabScriptLeftHand.SetItemRotationToDefault();
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
        Vector3 newVelocity = playerVelocity.normalized;
        playerRigidbody.AddForce((transform.forward * playerSpeed * newVelocity.x) + (transform.right * playerSpeed * newVelocity.z));
        //playerRigidbody.velocity = (transform.forward * Time.fixedDeltaTime * playerSpeed * playerVelocity.x) + (transform.right * Time.fixedDeltaTime * playerSpeed * playerVelocity.z);
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
        if (!grabPointLeft || !grabPointRight) return;
        
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(grabPointRight.position, .1f);
        Gizmos.DrawSphere(grabPointLeft.position, .1f);
    }
}
