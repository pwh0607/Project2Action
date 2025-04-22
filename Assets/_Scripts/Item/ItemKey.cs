using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/NormalKey")]
public class ItemKey : ItemData
{
    public ItemType ItemType => ItemType.KEY;

    [ReadOnly] public int index;              // 문 번호와 같다.
}
