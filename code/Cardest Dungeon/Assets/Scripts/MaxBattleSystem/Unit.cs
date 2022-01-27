using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public string unitName;
    public int unitLevel;

    public int damage;
    public int magic;

    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;
        //TODO:
        //Waere cool wenn der Slider so langsam runter geht wie halt bei Pokemon
        //Aktuell geht der Slider sofort um den dmg Wert runter

        if(currentHP <= 0)
        {
            return true;
            //Wenn die HP kleiner gleich 0 sind wird true returnet: Die Unit ist tot
        }
        else
        {
            return false;
            //Ansonsten false: Die Unit lebt noch
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
}
