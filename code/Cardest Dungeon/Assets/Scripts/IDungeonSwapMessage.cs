using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDungeonSwapMessage
{
    /// <summary>
    /// Will be called when this object's dungeon part is swapped.
    /// </summary>
    void OnDungeonSwap();
}
