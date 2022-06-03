using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
	public enum Effect
	{
		NONE

	}

	new public string name = "New Item";    // Name of the item
	public Sprite icon = null;              // Item icon

	public int defenseModifier;
	public int attackModifier;
	public int magicModifier;
	public bool fireball;
	public bool heal;
	public Effect effect;

	//// Called when the item is pressed in the inventory
	//public virtual void Use()
	//{
	//	// Use the item
	//	// Something might happen

	//	Debug.Log("Using " + name);
	//}

	//public void RemoveFromInventory()
	//{
	//	Inventory.instance.Remove(this);
	//}

}