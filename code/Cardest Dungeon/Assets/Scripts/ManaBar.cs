using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{

    public Slider slider;

    public void SetMaxMana(int magic)
    {
        slider.maxValue = magic;
        slider.value = magic;
    }

    public void SetMana(int magic)
    {
        slider.value = magic;
    }
}
