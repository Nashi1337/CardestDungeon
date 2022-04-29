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

	public int space = 10;  // Amount of slots in inventory

	// Current list of items in inventory
	public List<Item> items = new List<Item>();

    // Add a new item. If there is enough room we
    // return true. Else we return false.

    private void Start()
    {
		playerStats = FindObjectOfType<PlayerStats>();
		attackModifier = playerStats.Attack;
		defenseModifier = playerStats.Defense;
	}
    public bool Add(Item item)
	{

			// Check if out of space
			if (items.Count >= space)
			{
				Debug.Log("Not enough room.");
				return false;
			}

			items.Add(item);    // Add item to list

			attackModifier += item.attackModifier;
			defenseModifier += item.defenseModifier;

			playerStats.SetStats();

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

}