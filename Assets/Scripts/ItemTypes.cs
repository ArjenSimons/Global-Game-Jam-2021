using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTypes", menuName = "ScriptableObjects/ItemTypes")]
public class ItemTypes : ScriptableObject
{
    [SerializeField]
    private List<ItemType> types = new List<ItemType>();

    public List<ItemType> Types => types;
}
