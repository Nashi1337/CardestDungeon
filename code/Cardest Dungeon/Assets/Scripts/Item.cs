using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maybe remove abstract and just use prefabs for different items? Sounds smarter
/// </summary>
public class Item
{
    public string name;
    public readonly Sprite icon;

    //Possible changes the item does to the player need to be added as variables here

    public Item(string name, Sprite image)
    {
        this.name = name;
        icon = image;
    }

    /// <summary>
    /// Activates the effect of the item.
    /// </summary>
    /// <param name="player">Data type may need to be changed because this method is used in combat.</param>
    public void ActivateEffect(PlayerController player)
    {

    }

}
