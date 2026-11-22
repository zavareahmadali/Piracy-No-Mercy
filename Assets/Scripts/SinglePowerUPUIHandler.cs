using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SinglePowerUPUIHandler : MonoBehaviour
{
    public TMP_Text titleText;
    public Image image;
    public TMP_Text descriptionText;
    public TMP_Text levelText;

    private PowerUpInfoSO powerUpInfoSO;
    public void Initialize(PowerUpInfoSO powerUpInfo)
    {
        powerUpInfoSO = powerUpInfo;
        titleText.text = powerUpInfo.title;
        image.sprite = powerUpInfo.icon;
        descriptionText.text = powerUpInfo.description;
        levelText.text = (powerUpInfo.level + 1).ToString();

    }

    public void OnSelected()
    {
        FindAnyObjectByType<PowerUpPanelHandler>().OnPowerUpSelected(powerUpInfoSO);
    }

}
