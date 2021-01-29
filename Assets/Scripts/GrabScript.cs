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

    [Header("References")]
    [SerializeField]
    private Transform grabStartPoint;

    [SerializeField]
    private GameObject currentlyGrabbedObject = null;

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleSelectedItem();
    }

    // Handles what should happen when an item is in its selected list
    private void HandleSelectedItem()
    {
        if (currentlyGrabbedObject)
        {
            currentlyGrabbedObject.GetComponent<Rigidbody>().velocity = (transform.position - (currentlyGrabbedObject.transform.position + currentlyGrabbedObject.GetComponent<Rigidbody>().centerOfMass)) * magneticForce * Time.deltaTime;
        }
    }

    // When item needs to be selected
    public void SelectItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(grabStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, maxRayDistance))
        {
            if (hit.transform.tag == "PickupableItem")
            {
                currentlyGrabbedObject = hit.transform.gameObject;
            }
        }
    }

    // When item needs to be deselected
    public void DeselectItem()
    {
        if (!currentlyGrabbedObject) return;

        currentlyGrabbedObject = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(grabStartPoint.position, transform.TransformDirection(Vector3.forward) * maxRayDistance);
    }
}
