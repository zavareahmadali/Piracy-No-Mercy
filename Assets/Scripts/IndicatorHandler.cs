using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
public class IndicatorHandler : MonoBehaviour
{
    public SpriteRenderer visual;
    public TMP_Text indicatorText;
    public Sprite GoldSprite;
    public Sprite XPSprite;
    [SerializeField] public Dictionary<LootHandler.LootType, Sprite> indicatorSprites;

    void Start()
    {
        
    }

    public void Initialize (LootHandler.LootType type, string text)
    {
        visual.sprite = indicatorSprites[type];
        if (indicatorText != null)
        {
            indicatorText.text = text;
        }
        Destroy(gameObject, 2f); // Destroy the indicator after 2 seconds
    }
}
