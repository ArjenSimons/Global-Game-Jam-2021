using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrashShute : MonoBehaviour
{
    [SerializeField] private List<GameObject> items = null;
    [SerializeField] [Range(10, 100)] private float avgThrowForce = 50;
    [SerializeField] [Range(5, 20)] private float throwForceDeviation = 10;

    private List<GameObject> itemsToDrop = null;

    private void Start()
    {
        itemsToDrop = items;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            DropItem();
        }
    }

    private void DropItem()
    {
        Vector3 itemPos = transform.position;
        Quaternion itemRot = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        GameObject item = Instantiate(GetRandomItem(), transform);

        Rigidbody itemRB = item.GetComponent<Rigidbody>();

        float deviation = Random.Range(throwForceDeviation * -1, throwForceDeviation);
        float force = avgThrowForce + deviation;
        itemRB.AddForce(Vector3.down * force * 10);

        item.transform.rotation = itemRot;
    }

    private GameObject GetRandomItem()
    {
        if (items.Count <= 0)
        {
            Debug.LogWarning("No items left to drop");
            return null;
        }
        int randomIndex = Random.Range(0, items.Count);
        GameObject item = itemsToDrop[randomIndex];

        //itemsToDrop.RemoveAt(randomIndex);

        return item;
    }
}
