[System.Serializable]
public struct LostItem
{
    public ItemType ItemType;
    public ItemColor ItemColor;

    public LostItem(ItemType itemType, ItemColor itemColor)
    {
        ItemType = itemType;
        ItemColor = itemColor;
    }

    public override string ToString()
    {
        return string.Format("{0} {1}", ItemColor.DisplayName, ItemType.DisplayName);
    }
}