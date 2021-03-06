using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrashShute : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [Range(10, 100)] private float avgThrowForce = 50;

    [SerializeField] [Range(5, 20)] private float throwForceDeviation = 10;

    public void DropItem(LostItem itemToDrop)
    {
        Vector3 itemPos = transform.position;
        Quaternion itemRot = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        Color color = itemToDrop.ItemColor.Color;

        GameObject itemObj = Instantiate(itemToDrop.ItemType.PrefabItem, transform);
        Item item = itemObj.GetComponent<Item>();
        item.SetColor(itemToDrop.ItemColor.Color);
        item.LostItem = itemToDrop;
        item.GetComponent<GlowObjectGroup>().SetColor(color);

        Rigidbody itemRB = itemObj.GetComponent<Rigidbody>();

        float deviation = Random.Range(throwForceDeviation * -1, throwForceDeviation);
        float force = avgThrowForce + deviation;
        itemRB.AddForce(Vector3.down * force * 10);

        itemObj.transform.rotation = itemRot;
    }
}