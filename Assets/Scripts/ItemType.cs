using UnityEngine;

[CreateAssetMenu(fileName = "ItemType", menuName = "ScriptableObjects/ItemType")]
public class ItemType : ScriptableObject
{
    [SerializeField]
    private GameObject prefabItem = null;

    [SerializeField]
    private string displayName = "Suitcase";
}
