using UnityEngine;
using UnityEngine.UI;

public class IconFactory : BehaviourSingleton<IconFactory>
{
    [Header("Icon Prefab")] [SerializeField] ItemIcon iconPrefab;
    protected override bool IsDontDestroy() => false;
    public ItemIcon CreateItemIcon(Item item){
        ItemIcon icon = Instantiate(iconPrefab).GetComponent<ItemIcon>();
        icon.item = item;
        icon.GetComponent<Image>().sprite = item.data.sprite;
        
        return icon;
    }
}