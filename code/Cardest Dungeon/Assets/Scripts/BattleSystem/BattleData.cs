using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Place any data in this class that is supposed to be transfered between the dungeon and the battle scene. 
/// All variables here need to be either static or const in order to be not deleted upon scene loading.
/// This and the BattleData script should be merged.
/// </summary>
public static class BattleData
{
    public static GameObject[] enemiesToLoad;
    public static GameObject playerToLoad;
    public static Vector3 playerPositionBeforeFight;
}
