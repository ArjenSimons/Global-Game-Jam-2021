using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform trackTarget = null;

    [SerializeField]
    private Transform cameraObject = null;

    [SerializeField]
    private float lerpSpeed = 0.6f;

    private Vector3 lookPosition;

    private void Update()
    {
        lookPosition = Vector3.Lerp(lookPosition, trackTarget.position, lerpSpeed);
        cameraObject.LookAt(lookPosition);
    }
}
