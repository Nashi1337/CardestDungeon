using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This represents an in-game door which can be opened by a key. Complementary classes are Key and KeyHolder.
/// </summary>
public class KeyDoor : MonoBehaviour
{
    [SerializeField] 
    private Key.KeyType keyType;

    /// <summary>
    /// 
    /// </summary>
    /// <returns>The required type of key that is needed in order to open this door.</returns>
    public Key.KeyType GetKeyType()
    {
        return keyType;
    }

    /// <summary>
    /// Opens this door.
    /// </summary>
    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }

}
