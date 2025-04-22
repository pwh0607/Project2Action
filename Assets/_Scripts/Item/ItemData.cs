using UnityEngine;

public enum ItemType{ NONE, KEY, CONSUMABLE}

[CreateAssetMenu(menuName = "Item/NormalKey")]
public abstract class ItemData : ScriptableObject
{
    public ItemType ItemType{get;}
    public Sprite sprite;
    public string itemName;
    public string information;
}
