using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthStatusData", menuName = "StatusObjects/Health")]
public class CharacterStatus : ScriptableObject
{
    public string charName = "name";
    public Vector3 position = new Vector3();

    public GameObject characterGameObject;
    public int level = 1;
    public float maxHealth = 100;
    public float curHealth = 100;

    /*void methode()
    {
        position = PlayerController.Current.transform.position;
    }
    */
}
