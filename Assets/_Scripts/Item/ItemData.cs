using UnityEngine;

public enum ItemType{ NONE, KEY, CONSUMABLE}

[CreateAssetMenu(menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType ItemType{get;}
    public Sprite sprite;
    public GameObject itemModel;

    public string itemName;
    public string information;
}