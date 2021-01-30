using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackPos : MonoBehaviour
{
    [SerializeField]
    private Image image = null;

    [SerializeField]
    private TMP_Text text = null;

    [SerializeField]
    private float alphaLerpValue = 0.05f;

    [SerializeField]
    private float alphaWhenCenter = 0.1f;

    [SerializeField]
    private float alphaNormal = 1f;

    private Vector3 screenPosition;
    private Vector3 viewportPosition;
    private float alpha;

    public Transform TrackedObject { get; set; }

    private void Update()
    {
        if (TrackedObject != null)
        {
            screenPosition = Vector3.Lerp(screenPosition, Camera.main.WorldToScreenPoint(TrackedObject.position), 0.5f);
            transform.position = screenPosition;
            viewportPosition = Camera.main.WorldToViewportPoint(TrackedObject.transform.position);
        }

        bool atCenter = viewportPosition.x > 0.45f && viewportPosition.x < 0.55f && viewportPosition.y > 0.45f && viewportPosition.y < 0.55f
            && viewportPosition.z > 0;

        float targetAlpha = atCenter ? alphaWhenCenter : alphaNormal;
        alpha = TrackedObject != null && viewportPosition.z > 0 ? Mathf.Lerp(alpha, targetAlpha, alphaLerpValue) : 0f;
        Color color = image.color;
        color.a = alpha;
        image.color = color;
        text.color = color;
    }
}
