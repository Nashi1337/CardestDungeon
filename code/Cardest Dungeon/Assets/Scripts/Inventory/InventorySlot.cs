using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public GameObject isNotMergableEffect;
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
        if (Inventory.instance.merge_canCardsBeSelected && item != null && item.isMergable)
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

    public void AddIsNotMergableBorder()
    {
        isNotMergableEffect = Instantiate(Inventory.instance.isNotMergablePrefab, transform);
    }

    public void RemoveIsNotMergableBorder()
    {
        Destroy(isNotMergableEffect);
        isNotMergableEffect = null;
    }
}
