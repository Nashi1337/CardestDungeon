using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This represents an in-game key. Complementary classes are KeyDoor and KeyHolder.
/// </summary>
public class Key : MonoBehaviour
{

    [SerializeField]
    private KeyType keyType;
    /// <summary>
    /// keys have different codes as well as doors
    /// </summary>
    public enum KeyType
    {
        Red,
        Green,
        Blue,
        Gold,
        Black
    }

    public KeyType GetKeyType()
    {
        return keyType;
    }

}
