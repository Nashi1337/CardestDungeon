using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	PlayerStats playerStats;
    // Callback which is triggered when
    // an item gets added/removed.
    public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	private int attackModifier;
	private int defenseModifier;
	private int magicModifier;
	public bool fireball;
	public bool heal;
	public int heals;

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