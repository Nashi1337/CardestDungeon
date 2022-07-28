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
    public Sprite defaultIcon;

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

        icon.sprite = defaultIcon;
        //icon.enabled = false;
    }

    /// <summary>
    /// Add the is-selected-border if not there, yet and vice versa
    /// </summary>
    public void SwitchSelected()
    {
        if (((Inventory.instance.merge_canCardsBeSelected && item.isMergable) || Inventory.instance.remove_canCardsBeSelected) && item != null)
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
