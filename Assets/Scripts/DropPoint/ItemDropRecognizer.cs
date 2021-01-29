using UnityEngine;

public class ItemDropRecognizer : MonoBehaviour
{
    public GameObject RecognizedItem { get; private set; }

    public bool HasRecogizedItem
    {
        get { return RecognizedItem != null; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LostItem"))
        {
            RecognizedItem = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LostItem") && other.gameObject == RecognizedItem)
        {
            RecognizedItem = null;
        }
    }
}