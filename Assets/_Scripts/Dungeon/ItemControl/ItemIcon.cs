using UnityEngine;
using UnityEngine.UI;

//구조 : ItemData > Item > ItemIcon
public class ItemIcon : MonoBehaviour
{
    public Item item;
    private Sprite sprite;

    void Start()
    {
        sprite = GetComponent<Image>().sprite;
    }

    public void SetData(Item item){
        this.item = item;
        sprite = item.data.sprite;
    }
}