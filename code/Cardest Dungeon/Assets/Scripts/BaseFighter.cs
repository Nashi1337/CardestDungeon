using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Every Script that is supposed to represent a fighter in a fight needs to inherit 
/// from this class
/// </summary>
public abstract class BaseFighter : MonoBehaviour
{
    /// <summary>
    /// Holds all stats for the player
    /// </summary>
    public struct Stats
    {
        public int attack;
        public int defense;
        public int honor; //Just a fun value. No actual impact on game aon. (as of now)

        /// <summary>
        /// Adds another stat.
        /// </summary>
        /// <param name="add"></param>
        public void Add(Stats add)
        {
            attack += add.attack;
            defense += add.defense;
            honor += add.honor;

            attack = attack < 0 ? 0 : attack;
            defense = defense < 0 ? 0 : defense;
        }
    }

    public Stats stats;
}
