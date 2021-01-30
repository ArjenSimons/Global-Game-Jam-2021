using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour
{
    [Header("Setting"), Range(0f,1000f)]
    [SerializeField]
    private float magneticForce = 1f;

    [SerializeField, Range(.1f, 10f)]
    private float maxRayDistance = 2f;

    [SerializeField, Range(.1f, 1f)]
    private float itemSlerpRotationSpeed = .1f;

    [SerializeField]
    private bool alwaysKeepDefaultRotation = false;

    [SerializeField]
    private LayerMask grabRayLayermask = 0;

    [Header("References")]
    [SerializeField]
    private Transform grabStartPoint;

    [SerializeField]
    private Transform grabEmptyPoint;

    [SerializeField]
    private Transform formTransform;

    [SerializeField]
    private GameObject player;

    private BoxCollider playerCollider;
    private Rigidbody playerRigidbody;

    [SerializeField]
    private GameObject currentlyGrabbedObject = null;

    void Awake()
    {
        if (player)
        {
            playerCollider = player.GetComponent<BoxCollider>();
            playerRigidbody = player.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!currentlyGrabbedObject) return;

        if (currentlyGrabbedObject.TryGetComponent(out Form form))
        {
            HandleSelectedItem(formTransform, true);
        } 
        else
        {
            HandleSelectedItem(grabEmptyPoint);
        }
    }

    // Handles what should happen when an item is in its selected list
    private void HandleSelectedItem(Transform transform, bool forceRotation = false)
    {
        Rigidbody currentObjectRigidbody = currentlyGrabbedObject.GetComponent<Rigidbody>();

        if (alwaysKeepDefaultRotation || forceRotation )
        {
            SetItemRotationToDefault(transform);
        }

        currentObjectRigidbody.velocity = (transform.position - (currentlyGrabbedObject.transform.position + currentObjectRigidbody.centerOfMass)) * magneticForce * Time.deltaTime + playerRigidbody.velocity;
        currentObjectRigidbody.angularVelocity = Vector3.zero;
    }

    // When item needs to be selected
    public void SelectItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(grabStartPoint.position, grabStartPoint.TransformDirection(Vector3.forward), out hit, maxRayDistance, grabRayLayermask))
        {
            switch (hit.transform.tag)
            {
                case "PickupableItem":
                    Physics.IgnoreCollision(playerCollider, hit.collider, true);
                    currentlyGrabbedObject = hit.transform.gameObject;
                    break;
            }
        }
    }

    // When item needs to be deselected
    public void DeselectItem()
    {
        if (!currentlyGrabbedObject) return;

        Physics.IgnoreCollision(playerCollider, currentlyGrabbedObject.GetComponent<Collider>(), false);
        currentlyGrabbedObject = null;
    }

    public void SetItemRotationToDefault(Transform transform)
    {
        if (!currentlyGrabbedObject) return;

        Quaternion defaultRotation = transform.rotation;

        currentlyGrabbedObject.transform.rotation = Quaternion.Slerp(currentlyGrabbedObject.transform.rotation, defaultRotation, (itemSlerpRotationSpeed*10) * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        if (!grabEmptyPoint || !grabStartPoint) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(grabStartPoint.position, grabStartPoint.TransformDirection(Vector3.forward) * maxRayDistance);
    }
}
