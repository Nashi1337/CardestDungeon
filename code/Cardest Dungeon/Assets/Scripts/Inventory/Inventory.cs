using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	#region Singleton

	public static Inventory instance;

	private void Awake()
	{
        if (instance != null)
        {
			return;
        }
		instance = this;

	}

	#endregion

	[SerializeField]
	private Text mergeButtonText;
	[SerializeField]
	private Sprite attackCardSprite;
	[SerializeField]
	private Sprite defenceCardSprite;
	[SerializeField]
	private Sprite magicCardSprite;
	private PlayerStats playerStats;
	private int attackModifier;
	private int defenseModifier;
	private int magicModifier;


	public bool fireball;
	public bool heal;
	public int heals;
    public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;
	public GameObject isSelectedPrefab;
	public bool canCardsBeSelected;

	public int space;  // Amount of slots in inventory, set via SerializeField in Scene(default 10)

	// Current list of items in inventory
	public List<Item> items = new List<Item>();

    // Add a new item. If there is enough room we
    // return true. Else we return false.

    private void Start()
    {
		playerStats = FindObjectOfType<PlayerStats>();
		attackModifier = 0;
		defenseModifier = 0;
		magicModifier = 0;
	}
    public bool Add(Item item)
	{
		Debug.Log("Space: " + space);
		Debug.Log("Item count: " + items.Count);
			// Check if out of space
			if (items.Count >= space)
			{
				Debug.Log("Not enough room.");
				return false;
			}

		items.Add(item);    // Add item to list

		attackModifier += item.attackModifier;
		defenseModifier += item.defenseModifier;
		magicModifier += item.magicModifier;

		if (item.fireball)
        {
			fireball=true;
		}
        if (item.heal)
        {
			heal = true;
			heals = 3;
        }

		playerStats.UpdateStats();

		// Trigger callback
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();

		return true;
	}

	// Remove an item
	public void Remove(Item item)
	{
		items.Remove(item);     // Remove item from list

		attackModifier -= item.attackModifier;
		defenseModifier -= item.defenseModifier;
		magicModifier -= item.magicModifier;

		if (item.fireball)
		{
			fireball = false;
		}
		if (item.heal)
		{
			heal = false;
		}

		// Trigger callback
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}

	private EffectItem MergeItem(Item[] items, EffectItem effectItem)
    {
		int attack = 0, defence = 0, magic = 0;

		foreach(Item item in items)
        {
			attack += item.attackModifier;
			defence += item.defenseModifier;
			magic += item.magicModifier;
        }

		EffectItem mergedItem = new EffectItem();

		if(attack >= defence && attack >= magic)
        {
			mergedItem.attackModifier = attack;
			mergedItem.icon = attackCardSprite;

        }
		else if(defence >= magic)
        {
			mergedItem.defenseModifier = defence;
			mergedItem.icon = defenceCardSprite;
		}
		else
        {
			mergedItem.magicModifier = magic;
			mergedItem.icon = magicCardSprite;
        }

		mergedItem.SetEffect(effectItem.GetEffect());

		return mergedItem;
    }

	public int GetAttackModifier()
    {
		return attackModifier;
    }

	public int GetDefenseModifier()
    {
		return defenseModifier;
    }

	public int GetMagicModifier()
    {
		return magicModifier;
    }

	public void OnMergeButtonPress()
    {
		if (!canCardsBeSelected)
		{
			canCardsBeSelected = true;
			mergeButtonText.text = "Merge selected cards";
		}
		else
        {
			canCardsBeSelected = false;
			InventorySlot[] allItems = GetComponentsInChildren<InventorySlot>();

			List<Item> allSelectedItems = new List<Item>();
			EffectItem effectItem = null;

			foreach(InventorySlot slot in allItems)
            {
				if(slot.Item != null && slot.selectedEffect != null)
                {
					//Hier weiter machen
					//if(typof(slot.Item))
					allSelectedItems.Add(slot.Item);
                }
            }

			MergeItem(allSelectedItems.ToArray(), effectItem);
        }
	}
}