using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public GameObject selectedEffect;
    public Item Item { get { return item; } }
    public Image icon;

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

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Inventory.instance.canCardsBeSelected)
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

    public void OnDrag(PointerEventData eventData)
    {

    }
}
