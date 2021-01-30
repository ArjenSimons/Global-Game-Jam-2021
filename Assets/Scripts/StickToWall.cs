using System.Collections.Generic;
using UnityEngine;

public class StickToWall : MonoBehaviour
{
    private Dictionary<Grabbable, float> grabbablesOnWall = new Dictionary<Grabbable, float>();

    [SerializeField, Min(0)]
    private float paperDefaultOffset = 0.01f;

    [SerializeField, Min(0)]
    private float paperVariableOffset = 0.05f;

    private void Update()
    {
        foreach (var pair in grabbablesOnWall)
        {
            HandleStick(pair.Key, pair.Value);
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
        float offset = paperDefaultOffset + Random.Range(0, paperVariableOffset);
        grabbablesOnWall.Add(grabbable, offset);
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
        float offset = grabbablesOnWall[grabbable];
        HandleStick(grabbable, offset);
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
