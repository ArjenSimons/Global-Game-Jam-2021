using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemColors", menuName = "ScriptableObjects/ItemColors")]
public class ItemColors : ScriptableObject
{
    [SerializeField]
    private List<ItemColor> colors = new List<ItemColor>();

    public List<ItemColor> Colors => colors;
}
