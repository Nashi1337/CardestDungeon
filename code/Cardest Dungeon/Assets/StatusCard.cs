using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class StatusCard : MonoBehaviour
{
    private Image cardRenderer;
    private int maxValue = 20;
    
    [SerializeField] private Image fillSprite;
    [SerializeField] private Sprite cardNoDmg; 
    [SerializeField] private Sprite cardSomeDmg; 
    [SerializeField] private Sprite cardLotsOfDmg;

    [SerializeField] private TextMeshProUGUI currentValue;

    private void Awake()
    {
        cardRenderer = GetComponent<Image>();
    }

    public void SetStatMax(int value)
    {
        maxValue = value;
        fillSprite.fillAmount = 1;
        
        currentValue.text = value.ToString();
        cardRenderer.sprite = cardNoDmg;
    }

    public void SetStat(int value)
    {
        fillSprite.fillAmount = (float)value / maxValue;

        if (fillSprite.fillAmount > 0.66f)
        {
            cardRenderer.sprite = cardNoDmg;
        } else if (fillSprite.fillAmount < 0.33f)
        {
            cardRenderer.sprite = cardLotsOfDmg;
        }
        else
        {
            cardRenderer.sprite = cardSomeDmg;
        }

        currentValue.text = value.ToString();
    }
}
