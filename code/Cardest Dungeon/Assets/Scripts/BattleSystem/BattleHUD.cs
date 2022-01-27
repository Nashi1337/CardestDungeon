using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;

    /// <summary>
    /// Adds a fighter to this HUD. And saves a reference in the fighter to this HUD.
    /// </summary>
    /// <param name="fighter"></param>
    public void SetHUD(Fighter fighter)
    {
        nameText.text = fighter.name;
        levelText.text = "Lvl " + fighter.GetLevel();
        hpSlider.maxValue = fighter.GetStatus().health;
        hpSlider.value = hpSlider.maxValue;

        fighter.BattleHUD = this;
    }

    public void SetHealth(int health)
    {
        hpSlider.value = health;
    }
}
