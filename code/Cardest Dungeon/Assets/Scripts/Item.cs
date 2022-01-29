using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an Item. It is mainly used by the player and the inventory.
/// </summary>
public class Item : MonoBehaviour
{
    public Effects effects;

    /// <summary>
    /// Activates the effect of the item. However takes up the item system again should reconsider the existence of this question.
    /// </summary>
    /// <param name="player">Data type may need to be changed because this method is used in combat.</param>
    public void ActivateEffect(PlayerController player)
    {

    }

    /// <summary>
    /// Represents the additional bonuses an item gives. Should this be merged with Fighter.status in some way?
    /// </summary>
    [System.Serializable]
    public struct Effects
    {
        public int attack;
        public int defense;
        public int honor; //Just a fun value. No actual impact on game aon. (as of now)


        /// <summary>
        /// Returns a struct that is equivalent to the stat-changes this item has
        /// </summary>
        /// <returns></returns>
        /// kann ich des löschen yannik(y/n)?
        //public BaseFighter.Stats ToStats()
        //{
        //    BaseFighter.Stats stats = new BaseFighter.Stats();

        //    stats.attack = attack;
        //    stats.defense = defense;
        //    stats.honor = honor;

        //    return stats;
        //}
    }

}
