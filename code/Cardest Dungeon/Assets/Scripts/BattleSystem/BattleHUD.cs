using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// An Intermediary in order to pass information about a specific fighter to a specific HUD.
/// Each fighter has a reference to its HUD for easier access.
/// </summary>
public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;

    /// <summary>
    /// Adds a fighter to this HUD. And saves a reference in the fighter to this HUD.
    /// Also sets all important parameters that the HUD displays.
    /// </summary>
    /// <param name="fighter">The fighter which should be bound to this HUD</param>
    public void SetHUD(Fighter fighter)
    {
        nameText.text = fighter.name;
        levelText.text = "Lvl " + fighter.GetLevel();
        hpSlider.maxValue = fighter.GetStatus().health;
        hpSlider.value = hpSlider.maxValue;

        fighter.BattleHUD = this;
    }

    /// <summary>
    /// Updates the visual health bar by setting the health of the bound fighter.
    /// </summary>
    /// <param name="health">The up-to-date health points which will be displayed by the HUD</param>
    public void SetHealth(int health)
    {
        hpSlider.value = health;
    }
}
