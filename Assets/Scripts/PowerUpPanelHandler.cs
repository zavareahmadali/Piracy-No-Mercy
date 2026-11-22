using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpPanelHandler : MonoBehaviour
{
    public SinglePowerUPUIHandler singlePowerUpUIHandler;
    public Image backgroundImage;
    private List<GameObject> powerupsCreated;
    private PlayerPowerUPHandler PlayerPowerUPHandler;

    private List<PowerUpTypes> shownPowerup;
    
    private void Awake()
    {
        PlayerPowerUPHandler = FindAnyObjectByType<PlayerPowerUPHandler>();
        
    }
    private void Start()
    {
        //XPHandler.OnLevelUpdated += OnLevelUpdated;
        powerupsCreated = new List<GameObject>();
    }

    private void OnDisable()
    {
        //XPHandler.OnLevelUpdated -= OnLevelUpdated;
    }
    private void OnLevelUpdated(int level)
    {
        Time.timeScale = 0;
        backgroundImage.gameObject.SetActive(true);

        List<PowerUpTypes> noneMaxlevelPowerUps = new List<PowerUpTypes>();
        noneMaxlevelPowerUps = PlayerPowerUPHandler.GetNoneMaxLevelPowerUpsList();
        int maxNumberOfPowerupsToCreate = noneMaxlevelPowerUps.Count > 3 ? 3 : noneMaxlevelPowerUps.Count;

        shownPowerup = new List<PowerUpTypes>(maxNumberOfPowerupsToCreate);
        PowerUpTypes randomType = PowerUpTypes.HpIncrease;

        while (shownPowerup.Count != maxNumberOfPowerupsToCreate)
        {
            randomType = (PowerUpTypes)Enum.ToObject(typeof(PowerUpTypes), UnityEngine.Random.Range(0, PowerUpTypes.GetValues(typeof(PowerUpTypes)).Length));

            if (noneMaxlevelPowerUps.Contains(randomType) && !shownPowerup.Contains(randomType))
            {
                var p = CreateSinglePowerUp(randomType);
                p.transform.SetParent(this.transform, false);
                powerupsCreated.Add(p.gameObject);
                shownPowerup.Add(randomType);
            }
        }

    }

    public void OnPowerUpSelected(PowerUpInfoSO powerUpInfo)
    {
        //Debug.Log("Selected PowerUp: " + powerUpInfo.title);
        backgroundImage.gameObject.SetActive(false);
        PlayerPowerUPHandler.AddPowerup(powerUpInfo);
        GameManagerHandler.isGamePaused = false;
        foreach (var p in powerupsCreated)
        {
            Destroy(p.gameObject);
        }
        Time.timeScale = 1;
    }
    public SinglePowerUPUIHandler CreateSinglePowerUp(PowerUpTypes powerUpType)
    {
        SinglePowerUPUIHandler p = Instantiate(singlePowerUpUIHandler, transform);
        p.Initialize(PlayerPowerUPHandler.GetPowerUpInfo(powerUpType));
        return p;
    }


}
