using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
	private int[] possibleCardStrengthValuesASCENDING;
	[SerializeField]
	private Text mergeButtonText;
	[SerializeField]
	private Text removeButtonText;
	[SerializeField]
	private Sprite[] attackCardSprites;
	[SerializeField]
	private Sprite[] defenceCardSprites;
	[SerializeField]
	private Sprite[] magicCardSprites;

	private PlayerStats playerStats;
	private int attackModifier;
	private int defenseModifier;
	private int magicModifier;
	private int hpModifier;

	public bool fireball;
	public bool heal;
	public bool merge_canCardsBeSelected;
	public bool remove_canCardsBeSelected;
	public int heals;
	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;
	public GameObject isSelectedPrefab;
	public GameObject isNotMergablePrefab;

	public int space;  // Amount of slots in inventory, set via SerializeField in Scene(default 10)

	// Current list of items in inventory
	public List<Item> items = new List<Item>();

	public void Initialize()
	{
		playerStats = FindObjectOfType<PlayerStats>();
		attackModifier = 0;
		defenseModifier = 0;
		magicModifier = 0;
		hpModifier = 0;

		LoadInventoryFromPlayerStats();
	}

	public bool Add(Item item)
	{
		// Check if out of space
		if (items.Count >= space)
		{
			//Debug.Log("Not enough room.");
			return false;
		}

		items.Add(item);    // Add item to list

		playerStats.IncreaseHighScore(item.attackModifier + item.defenseModifier + item.magicModifier + item.hpModifier);

		attackModifier += item.attackModifier;
		defenseModifier += item.defenseModifier;
		magicModifier += item.magicModifier;
		hpModifier += item.hpModifier;

		if (item.fireball)
		{
			fireball = true;
			playerStats.IncreaseHighScore(5);
		}
		if (item.heal)
		{
			heal = true;
			heals = 3;
			playerStats.IncreaseHighScore(10);
		}

		playerStats.UseMana(-item.magicModifier);
		playerStats.Heal(item.hpModifier);
		playerStats.UpdateStats();

		// Trigger callback
		if (onItemChangedCallback != null)
		{
			onItemChangedCallback.Invoke();
		}

		return true;
	}

	// Remove an item
	public void Remove(Item item)
	{
		items.Remove(item);     // Remove item from list

		attackModifier -= item.attackModifier;
		defenseModifier -= item.defenseModifier;
		magicModifier -= item.magicModifier;
		hpModifier -= item.hpModifier;

		if (item.fireball)
		{
			fireball = false;
		}
		if (item.heal)
		{
			heal = false;
		}

		playerStats.UpdateStats();


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
		int attack = 0, defence = 0, magic = 0, hpPlus = 0;

		foreach (Item item in items)
		{
			attack += item.attackModifier;
			defence += item.defenseModifier;
			magic += item.magicModifier;
			hpPlus += item.hpModifier;
		}

		Item mergedItem = ScriptableObject.CreateInstance<Item>();
		mergedItem.isMergable = true;

		if (attack >= defence && attack >= magic)
		{
			mergedItem.attackModifier = ReturningClosestStrengthValues(attack);
			if (mergedItem.attackModifier == 8)
            {
				mergedItem.icon = attackCardSprites[3];
				mergedItem.isMergable = false;
				mergedItem.hpModifier = 5;
            }
			else if (mergedItem.attackModifier == 5)
            {
				mergedItem.icon = attackCardSprites[2];
				mergedItem.hpModifier = 2;
            }
			else if (mergedItem.attackModifier == 2)
				mergedItem.icon = attackCardSprites[1];
			else
				mergedItem.icon = attackCardSprites[0];

		}
		else if (defence >= magic)
		{
			mergedItem.defenseModifier = ReturningClosestStrengthValues(defence);
			if (mergedItem.defenseModifier == 8)
            {
				mergedItem.icon = defenceCardSprites[3];
				mergedItem.isMergable = false;
				mergedItem.hpModifier = 5;
            }
			else if (mergedItem.defenseModifier == 5)
            {
				mergedItem.icon = defenceCardSprites[2];
				mergedItem.hpModifier = 2;
            }
			else if (mergedItem.defenseModifier == 2)
				mergedItem.icon = defenceCardSprites[1];
			else
				mergedItem.icon = defenceCardSprites[0];
		}
		else
		{
			mergedItem.magicModifier = ReturningClosestStrengthValues(magic);
			if (mergedItem.magicModifier == 8)
            {
				mergedItem.icon = magicCardSprites[3];
				mergedItem.isMergable = false;
				mergedItem.hpModifier = 5;
            }
			else if (mergedItem.magicModifier == 5)
            {
				mergedItem.icon = magicCardSprites[2];
				mergedItem.hpModifier = 2;
            }
			else if (mergedItem.magicModifier == 2)
				mergedItem.icon = magicCardSprites[1];
			else
				mergedItem.icon = magicCardSprites[0];
		}

		if (effectItem != null)
		{
			mergedItem.effect = effectItem.effect;
		}

		return mergedItem;
	}

	private int ReturningClosestStrengthValues(int value)
	{

		for (int i = possibleCardStrengthValuesASCENDING.Length - 1; i >= 0; i--)
		{
			if (possibleCardStrengthValuesASCENDING[i] <= value)
			{
				return possibleCardStrengthValuesASCENDING[i];
			}
		}

		throw new System.Exception("Lol was ist hier passiert");
	}

	public void OnMergeButtonPress()
	{
		InventorySlot[] allItems = GetComponentsInChildren<InventorySlot>();
		if (!merge_canCardsBeSelected && !remove_canCardsBeSelected)
		{
			merge_canCardsBeSelected = true;
			mergeButtonText.text = "MERGE\nSELECTED\nCARDS";

			foreach (InventorySlot slot in allItems)
			{
				if (slot.item != null && !slot.item.isMergable)
				{
					slot.AddIsNotMergableBorder();
				}
			}
		}
		else
		{
			List<Item> allSelectedItems = new List<Item>();
			Item effectItem = null;

			foreach (InventorySlot slot in allItems)
			{
				if (slot.Item != null && slot.selectedEffect != null)
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
				slot.RemoveIsNotMergableBorder();
			}

			mergeButtonText.text = "START\nMERGING";
			merge_canCardsBeSelected = false; //Diese Zeile muss immer nach slot.SwitchSelected() stehen

			if (allSelectedItems.Count > 1)
			{
				Item mergedItem = MergeItem(allSelectedItems.ToArray(), effectItem);
				foreach (Item item in allSelectedItems)
				{
					Remove(item);
				}
				Add(mergedItem);
			}
			else
			{
				//Here a text should be displayed that you cannot merge
				Debug.LogWarning("Missing text message for player. See Code comment");
			}
		}

		playerStats.UpdateStats();
	}

	public void OnRemoveButtonPress()
	{
		if (!remove_canCardsBeSelected && !merge_canCardsBeSelected)
		{
			remove_canCardsBeSelected = true;
			removeButtonText.text = "REMOVE\nSELECTED\nCARDS";
		}
		else
		{
			InventorySlot[] allItems = GetComponentsInChildren<InventorySlot>();
			List<Item> allSelectedItems = new List<Item>();

			foreach (InventorySlot slot in allItems)
			{
				if (slot.Item != null && slot.selectedEffect != null)
				{
					allSelectedItems.Add(slot.Item);
					slot.SwitchSelected();
				}
			}

			removeButtonText.text = "START\nREMOVING\nCARDS";
			remove_canCardsBeSelected = false; //Diese Zeile muss immer nach slot.SwitchSelected() stehen

			if (allSelectedItems.Count >= 1)
			{
				foreach (Item item in allSelectedItems)
				{
					Remove(item);
				}
			}
		}

		playerStats.UpdateStats();
	}

	public void ResetButtons()
	{
		InventorySlot[] allItems = GetComponentsInChildren<InventorySlot>();

		foreach (InventorySlot slot in allItems)
		{
			if (slot.Item != null && slot.selectedEffect != null)
			{
				slot.SwitchSelected();
			}
			slot.RemoveIsNotMergableBorder();
		}

		removeButtonText.text = "START\nREMOVING\nCARDS";
		remove_canCardsBeSelected = false; //Diese Zeile muss immer nach slot.SwitchSelected() stehen
		mergeButtonText.text = "START\nMERGING";
		merge_canCardsBeSelected = false; //Diese Zeile muss immer nach slot.SwitchSelected() stehen
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

	public int GetHPModifier()
    {
		return hpModifier;
    }

	public Item[] GetAllItems()
	{
		return items.ToArray();
	}

	public void SaveInventoryToPlayerStats()
	{
		string data = "";
		Item[] allItems = GetAllItems();
		foreach (Item item in allItems)
		{
			Item.SERIALIZABLE_Item si = item.CreateSERIALIZABLE_Item();
			data += JsonUtility.ToJson(si) + "#";
		}

		PlayerPrefs.SetString("Inventory", data);
	}

	/// <summary>
	/// WARNING: THIS OVERWRITES ALL ITEMS THAT ARE CURRENTLY IN THE INVENTORY
	/// </summary>
	public void LoadInventoryFromPlayerStats()
	{
		Item[] oldItems = items.ToArray();
		foreach (Item item in oldItems)
		{
			Remove(item);
		}
		Item[] newItems = LoadItemsFromPlayerStats();
		foreach (Item item in newItems)
		{
			Add(item);
		}
	}

	private Item[] LoadItemsFromPlayerStats()
	{
		string data = PlayerPrefs.GetString("Inventory");


		string[] str_Items = data.Split(new char[] { '#' }, System.StringSplitOptions.RemoveEmptyEntries);
		Item[] items = new Item[str_Items.Length];
		for (int i = 0; i < str_Items.Length; i++)
		{
			Item.SERIALIZABLE_Item si = JsonUtility.FromJson<Item.SERIALIZABLE_Item>(str_Items[i]);
			items[i] = Item.CreateItem(si);
		}
		return items;
	}


}