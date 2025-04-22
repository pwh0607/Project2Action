using UnityEngine;

public class IconFactory : BehaviourSingleton<IconFactory>
{
    [Header("Icon Prefab")] [SerializeField] GameObject iconPrefab;
    protected override bool IsDontDestroy() => false;
    public GameObject CreateItemIcon(ItemData itemData){
        GameObject icon = null;

        icon.GetComponent<SpriteRenderer>().sprite = itemData.sprite;
        
        return icon;
    }
}
