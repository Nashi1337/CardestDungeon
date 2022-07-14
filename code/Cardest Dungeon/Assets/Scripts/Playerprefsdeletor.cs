using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerprefsdeletor : MonoBehaviour
{
    void OnEnable()
    {
        Debug.LogWarning("Playerprefsdeletor should only exist in the first dungeon");
        PlayerPrefs.DeleteKey("Inventory");
    }
}
