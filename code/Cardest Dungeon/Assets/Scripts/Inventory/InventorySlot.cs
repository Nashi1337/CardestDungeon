using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public GameObject selectedEffect;
    public Item Item { get { return item; } }
    public Image icon;

    public
    Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void SwitchSelected()
    {
        if (Inventory.instance.canCardsBeSelected && item != null)
        {
            if (selectedEffect == null)
            {
                selectedEffect = Instantiate(Inventory.instance.isSelectedPrefab, transform);
            }
            else
            {
                Destroy(selectedEffect);
            }
        }
    }
}
