using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This represents an class which is able to collect, hold and use keys. Complementary classes are KeyDoor and Key.
/// </summary>
public class KeyHolder : MonoBehaviour
{


    private List<Key.KeyType> keyList;

    [SerializeField]
    private GameObject iconKeyRed;
    [SerializeField]
    private GameObject iconKeyBlue;
    private DialogueManager dm;

    private void Awake()
    {
        keyList = new List<Key.KeyType>();
        Color temp = iconKeyBlue.GetComponent<Image>().color;
        temp.a = 0;
        iconKeyBlue.GetComponent<Image>().color = temp;
        
        temp = iconKeyRed.GetComponent<Image>().color;
        temp.a = 0;
        iconKeyRed.GetComponent<Image>().color = temp;

        dm = FindObjectOfType<DialogueManager>();

    }

    /// <summary>
    /// Adds a key of type keyType to this KeyHolder.
    /// </summary>
    /// <param name="keyType">The key type that will be added.</param>
    public void AddKey(Key.KeyType keyType)
    {
        Debug.Log("Added key: " + keyType);
        keyList.Add(keyType);
        if (keyType ==Key.KeyType.Blue)
        {
            Color temp = iconKeyBlue.GetComponent<Image>().color;
            temp.a = 1;
            iconKeyBlue.GetComponent<Image>().color = temp;


            dm.BlueKey();
        }
        if (keyType == Key.KeyType.Red)
        {
            Color temp = iconKeyRed.GetComponent<Image>().color;
            temp.a = 1;
            iconKeyRed.GetComponent<Image>().color = temp;

            dm.RedKey();
        }
    }

    /// <summary>
    /// Removes a key of type keyType from this KeyHolder.
    /// </summary>
    /// <param name="keyType">The key type that should be removed.</param>
    public void Removekey(Key.KeyType keyType)
    {
        keyList.Remove(keyType);
    }

    /// <summary>
    /// Checks if this Keyholder has a key of type keyType.
    /// </summary>
    /// <param name="keyType"></param>
    /// <returns>True, if such a key was found. Else false.</returns>
    public bool ContainsKey(Key.KeyType keyType)
    {
        return keyList.Contains(keyType);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "key")
        {
            Key key = collider.GetComponent<Key>();
            AddKey(key.GetKeyType());
            Destroy(key.gameObject);
        }

        KeyDoor keyDoor = collider.GetComponent<KeyDoor>();
        if (keyDoor != null)
        {
            if (ContainsKey(keyDoor.GetKeyType()))
            {
                // Currently holding Key to open this door
                Removekey(keyDoor.GetKeyType());
                keyDoor.OpenDoor();

                if (keyDoor.GetKeyType() == Key.KeyType.Blue)
                {
                    iconKeyBlue.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    dm.BlueDoor();

                }
                if (keyDoor.GetKeyType() == Key.KeyType.Red)
                {
                    iconKeyRed.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    dm.RedDoor();
                }
            }
        }
    }

}
