using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_MergeWithPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject attackPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(InputManager.GetActionDown(InputManager.action))
        {
            //Player flipping not taken into account, yet. Needs to be done after Merging this script with PlayerController
            GameObject attack = Instantiate(attackPrefab, transform);

            //If the sprite is flipped in x direction -1. else 1
            int flipfactor = PlayerController.Current.GetComponent<SpriteRenderer>().flipX ? -1: 1;
            
            attack.transform.localPosition = new Vector3(flipfactor * 1.5f, 0, 0);
        }
    }
}
