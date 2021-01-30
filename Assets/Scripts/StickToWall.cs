using System.Collections.Generic;
using UnityEngine;

public class StickToWall : MonoBehaviour
{
    private List<Grabbable> grabbablesOnWall = new List<Grabbable>();

    [SerializeField, Min(0)]
    private float paperMinOffset = 0.01f;

    [SerializeField, Min(0)]
    private float paperMaxOffset = 0.05f;

    private float OffsetIncrement => (paperMaxOffset - paperMinOffset) / grabbablesOnWall.Count;

    private void Update()
    {
        for (int i = 0; i < grabbablesOnWall.Count; i++)
        {
            HandleStick(grabbablesOnWall[i], paperMinOffset + OffsetIncrement * i);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Form form = other.GetComponent<Form>();
        if (form == null)
        {
            return;
        }

        Grabbable grabbable = other.GetComponent<Grabbable>();
        float offset = paperMinOffset + Random.Range(0, paperMaxOffset);
        grabbablesOnWall.Add(grabbable);
        HandleStick(grabbable, offset);

    }

    private void OnTriggerExit(Collider other)
    {
        Form form = other.GetComponent<Form>();
        if (form == null)
        {
            return;
        }

        Grabbable grabbable = other.GetComponent<Grabbable>();
        HandleStick(grabbable, paperMaxOffset);
        grabbablesOnWall.Remove(grabbable);
    }

    private void HandleStick(Grabbable grabbable, float offset)
    {
        if (!grabbable.IsGrabbed)
        {
            grabbable.GetComponent<Rigidbody>().isKinematic = true;
            grabbable.transform.rotation = Quaternion.LookRotation(transform.forward);
            Vector3 position = grabbable.transform.position;
            position.z = transform.position.z - offset;
            grabbable.transform.position = position;
        }
        else
        {
            grabbable.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
