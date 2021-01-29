using UnityEngine;

[CreateAssetMenu(fileName = "ItemColor", menuName = "ScriptableObjects/ItemColor")]
public class ItemColor : ScriptableObject
{
    [SerializeField]
    private Color color = Color.black;

    [SerializeField]
    private string displayName = "Black";

    public Color Color => color;
    public string DisplayName => displayName;
}
