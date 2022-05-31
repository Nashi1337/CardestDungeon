using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectItem : Item
{
    public enum Effect
    {
        NONE

    }

    [SerializeField]
    private Effect effect;

    public Effect GetEffect()
    {
        return effect;
    }

    public void SetEffect(Effect effect)
    {
        this.effect = effect;
    }
}
