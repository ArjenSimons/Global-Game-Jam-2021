using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrashShute : MonoBehaviour
{
    [SerializeField] [Range(10, 100)] private float avgThrowForce = 50;
    [SerializeField] [Range(5, 20)] private float throwForceDeviation = 10;

    private List<GameObject> itemsToDrop = null;

    public void DropItem(LostItem itemToDrop)
    {
        Vector3 itemPos = transform.position;
        Quaternion itemRot = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        GameObject itemObj = Instantiate(itemToDrop.ItemType.PrefabItem, transform);
        Item item = itemObj.GetComponent<Item>();
        item.SetColor(itemToDrop.ItemColor.Color);
        item.LostItem = itemToDrop;

        Rigidbody itemRB = itemObj.GetComponent<Rigidbody>();

        float deviation = Random.Range(throwForceDeviation * -1, throwForceDeviation);
        float force = avgThrowForce + deviation;
        itemRB.AddForce(Vector3.down * force * 10);

        itemObj.transform.rotation = itemRot;
    }
}
