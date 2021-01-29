using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrashShute : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<GameObject> items = null;

    [SerializeField] [Range(10, 100)] private float avgThrowForce = 50;
    [SerializeField] [Range(5, 20)] private float throwForceDeviation = 10;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private LostItemChannel itemDroppedChannel = null;

    private List<GameObject> itemsToDrop = null;

    public void DropItem(LostItem itemToDrop)
    {
        Vector3 itemPos = transform.position;
        Quaternion itemRot = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        Color color = itemToDrop.ItemColor.Color;
        GameObject item = Instantiate(itemToDrop.ItemType.PrefabItem, transform);
        item.GetComponent<ItemColorSetter>().SetColor(color);
        item.GetComponent<GlowObjectGroup>().SetColor(color);

        Rigidbody itemRB = item.GetComponent<Rigidbody>();

        float deviation = Random.Range(throwForceDeviation * -1, throwForceDeviation);
        float force = avgThrowForce + deviation;
        itemRB.AddForce(Vector3.down * force * 10);

        item.transform.rotation = itemRot;

        itemDroppedChannel.RaiseEvent(itemToDrop);
    }
}