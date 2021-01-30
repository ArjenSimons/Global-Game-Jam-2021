using BWolf.Utilities.AudioPlaying;
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

    [Header("Sound")]
    [SerializeField]
    private AudioCueSO PaperPickupAudioCue = null;

    [SerializeField]
    private AudioCueSO PaperLetGoAudioCue = null;

    [SerializeField]
    private AudioConfigurationSO config = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private AudioRequestChannelSO channel = null;

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

    public GameObject CurrentlyGrabbedObject { get; private set; }

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
        if (!CurrentlyGrabbedObject) return;

        if (CurrentlyGrabbedObject.TryGetComponent(out Form form))
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
        Rigidbody currentObjectRigidbody = CurrentlyGrabbedObject.GetComponent<Rigidbody>();

        if (alwaysKeepDefaultRotation || forceRotation )
        {
            SetItemRotationToDefault(transform);
        }

        currentObjectRigidbody.velocity = (transform.position - (CurrentlyGrabbedObject.transform.position + currentObjectRigidbody.centerOfMass)) * magneticForce * Time.fixedDeltaTime + playerRigidbody.velocity;
        currentObjectRigidbody.angularVelocity = Vector3.zero;
    }

    // When item needs to be selected
    public void SelectItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(grabStartPoint.position, grabStartPoint.TransformDirection(Vector3.forward), out hit, maxRayDistance))
        {
            switch (hit.transform.tag)
            {
                case "PickupableItem":
                    Physics.IgnoreCollision(playerCollider, hit.collider, true);
                    CurrentlyGrabbedObject = hit.transform.gameObject;

                    Grabbable grabbable = CurrentlyGrabbedObject.GetComponent<Grabbable>();
                    if (grabbable != null)
                    {
                        grabbable.IsGrabbed = true;
                        channel.RaiseEvent(config, PaperPickupAudioCue, grabbable.transform.position);
                    }
                    break;
            }
        }
    }

    // When item needs to be deselected
    public void DeselectItem()
    {
        if (!CurrentlyGrabbedObject) return;

        Physics.IgnoreCollision(playerCollider, CurrentlyGrabbedObject.GetComponent<Collider>(), false);
        Grabbable grabbable = CurrentlyGrabbedObject.GetComponent<Grabbable>();
        if (grabbable != null)
        {
            grabbable.IsGrabbed = false;
            channel.RaiseEvent(config, PaperLetGoAudioCue, grabbable.transform.position);
        }
        CurrentlyGrabbedObject = null;
    }

    public void ThrowItemsAway(float throwingForce)
    {
        if (!CurrentlyGrabbedObject) return;

        CurrentlyGrabbedObject.GetComponent<Rigidbody>().AddForce(grabStartPoint.TransformDirection(Vector3.forward) * throwingForce, ForceMode.VelocityChange);
        Physics.IgnoreCollision(playerCollider, CurrentlyGrabbedObject.GetComponent<Collider>(), false);
        Grabbable grabbable = CurrentlyGrabbedObject.GetComponent<Grabbable>();
        if (grabbable != null)
        {
            grabbable.IsGrabbed = false;
            channel.RaiseEvent(config, PaperLetGoAudioCue, grabbable.transform.position);
        }
        CurrentlyGrabbedObject = null;
    }

    public void SetItemRotationToDefault(Transform transform)
    {
        if (!CurrentlyGrabbedObject) return;

        Quaternion defaultRotation = transform.rotation;

        CurrentlyGrabbedObject.transform.rotation = Quaternion.Slerp(CurrentlyGrabbedObject.transform.rotation, defaultRotation, (itemSlerpRotationSpeed*10) * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        if (!grabEmptyPoint || !grabStartPoint) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(grabStartPoint.position, grabStartPoint.TransformDirection(Vector3.forward) * maxRayDistance);
    }
}
