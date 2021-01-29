using TMPro;
using UnityEngine;


public class Form : MonoBehaviour
{
    [SerializeField]
    private Collider paperCollider = null;

    [SerializeField]
    private Rigidbody rigidBody = null;

    [SerializeField]
    private TMP_Text text = null;

    private void Start()
    {
        paperCollider.enabled = false;
        //TODO: make object not grabbable by player
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void EnablePaper()
    {
        paperCollider.enabled = true;
        rigidBody.isKinematic = false;
        //TODO: make object grabbable by player
    }
}
