using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maybe remove abstract and just use prefabs for different items? Sounds smarter.
/// Yep, will be done the smorter way.
/// </summary>
public class Item : MonoBehaviour
{
    public Effects effects;

    /// <summary>
    /// Activates the effect of the item.
    /// </summary>
    /// <param name="player">Data type may need to be changed because this method is used in combat.</param>
    public void ActivateEffect(PlayerController player)
    {

    }

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
        public BaseFighter.Stats ToStats()
        {
            BaseFighter.Stats stats = new BaseFighter.Stats();

            stats.attack = attack;
            stats.defense = defense;
            stats.honor = honor;

            return stats;
        }
    }

}
