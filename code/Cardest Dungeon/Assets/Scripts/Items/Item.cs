using UnityEngine;
using UnityEngine.UI;

/* The base item class. All items should derive from this. */

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
	/// <summary>
	/// unused
	/// </summary>
	public enum Effect
	{
		NONE

	}

	new public string name = "New Item";    // Name of the item
	public Sprite icon = null;              // Item icon
	public Image mergePreview;

	public int defenseModifier;
	public int attackModifier;
	public int magicModifier;
	public int hpModifier;
	public bool fireball;
	public bool heal;
	public bool isMergable;
	public Effect effect;

	public SERIALIZABLE_Item CreateSERIALIZABLE_Item()
	{
		SERIALIZABLE_Item si = new SERIALIZABLE_Item();
		si.name = name;
		si.icon = icon;
		si.defenseModifier = defenseModifier;
		si.attackModifier = attackModifier;
		si.magicModifier = magicModifier;
		si.hpModifier = hpModifier;
		si.fireball = fireball;
		si.heal = heal;
		si.isMergable = isMergable;
		si.effect = effect;

		return si;
	}

	public static Item CreateItem(SERIALIZABLE_Item sERIALIZABLE_Item)
	{
		Item item = CreateInstance<Item>();
		item.name = sERIALIZABLE_Item.name;
		item.icon = sERIALIZABLE_Item.icon;
		item.defenseModifier = sERIALIZABLE_Item.defenseModifier;
		item.attackModifier = sERIALIZABLE_Item.attackModifier;
		item.magicModifier = sERIALIZABLE_Item.magicModifier;
		item.hpModifier = sERIALIZABLE_Item.hpModifier;
		item.fireball = sERIALIZABLE_Item.fireball;
		item.heal = sERIALIZABLE_Item.heal;
		item.isMergable = sERIALIZABLE_Item.isMergable;
		item.effect = sERIALIZABLE_Item.effect;

		return item;
	}

	/// <summary>
	/// Wrapperclass so that Item is serializable and deserializable because ScriptableObjects cannot be deserialized.
	/// </summary>
	[System.Serializable]
	public class SERIALIZABLE_Item
	{
		public string name = "New Item";    // Name of the item
		public Sprite icon = null;              // Item icon

		public int defenseModifier;
		public int attackModifier;
		public int magicModifier;
		public int hpModifier;
		public bool fireball;
		public bool heal;
		public bool isMergable;
		public Effect effect;
	}
}