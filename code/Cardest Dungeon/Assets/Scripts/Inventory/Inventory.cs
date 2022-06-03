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
	[SerializeField]
	private float mergeAmplifier;

	public bool fireball;
	public bool heal;
	public bool canCardsBeSelected;
	public int heals;
    public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;
	public GameObject isSelectedPrefab;

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
		//Debug.Log("Space: " + space);
		//Debug.Log("Item count: " + items.Count);
			// Check if out of space
			if (items.Count >= space)
			{
				//Debug.Log("Not enough room.");
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

	/// <summary>
	/// Sums up each the attack, defence and magic modifier. The returned card will only have the effect with the highest sum. If an effect Item is added the returned card will have a special effect
	/// </summary>
	/// <param name="items"></param>
	/// <param name="effectItem"></param>
	/// <returns></returns>
	private Item MergeItem(Item[] items, Item effectItem = null)
	{
		int attack = 0, defence = 0, magic = 0;

		foreach (Item item in items)
		{
			attack += item.attackModifier;
			defence += item.defenseModifier;
			magic += item.magicModifier;
		}

		Item mergedItem = ScriptableObject.CreateInstance<Item>();

		if (attack >= defence && attack >= magic)
		{
			mergedItem.attackModifier = Mathf.CeilToInt(attack * mergeAmplifier);
			mergedItem.icon = attackCardSprite;

		}
		else if (defence >= magic)
		{
			mergedItem.defenseModifier = Mathf.CeilToInt(defence * mergeAmplifier);
			mergedItem.icon = defenceCardSprite;
		}
		else
		{
			mergedItem.magicModifier = Mathf.CeilToInt(magic * mergeAmplifier);
			mergedItem.icon = magicCardSprite;
		}

		if (effectItem != null)
		{
			mergedItem.effect = effectItem.effect;
		}
		return mergedItem;
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
			InventorySlot[] allItems = GetComponentsInChildren<InventorySlot>();

			List<Item> allSelectedItems = new List<Item>();
			Item effectItem = null;

			foreach(InventorySlot slot in allItems)
            {
				if(slot.Item != null && slot.selectedEffect != null)
                {
					if (slot.Item.effect != Item.Effect.NONE && effectItem == null)
					{
						effectItem = slot.Item;
					}
					else
					{
						allSelectedItems.Add(slot.Item);
					}
					slot.SwitchSelected();
                }
            }
			
			mergeButtonText.text = "Activate selection";
			canCardsBeSelected = false; //Diese Zeile muss immer nach slot.SwitchSelected() stehen

			if (allSelectedItems.Count > 1)
			{
				Item mergedItem = MergeItem(allSelectedItems.ToArray(), effectItem);
				Add(mergedItem);
				foreach(Item item in allSelectedItems)
                {
					Remove(item);
                }
			}
			else
            {
				//Here a text should be displayed that you cannot merge
				Debug.LogWarning("Missing text message for player. See Code comment");
            }
		}
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

}