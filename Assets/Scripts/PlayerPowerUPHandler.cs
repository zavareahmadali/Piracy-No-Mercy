using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPowerUPHandler : MonoBehaviour
{
    [Serializable]
    public class PowerUpInfoHolder
    {
        public PowerUpTypes PowerUpType;
        public PowerUpInfoSO[] PowerUpArray;
        internal int currentLevel;
    }
    public PowerUpInfoHolder[] PowerUpInfos;
    private ShipMovementHandler shipMovementHandler;
    private PlayerHPHandler playerHPHandler;

    private void Awake()
    {
        shipMovementHandler = GetComponent<ShipMovementHandler>();
        playerHPHandler = GetComponent<PlayerHPHandler>();
    }
    void Start()
    {
        for (int i = 0; i < PowerUpInfos.Length; i++)
        {
            PowerUpInfos[i].currentLevel = -1;
        }
    }
    public void AddPowerup(PowerUpInfoSO powerup)
    {
        int powerUpInfoIndex = GetPowerUpInfoIndex(powerup.powerUpType);
        PowerUpInfos[powerUpInfoIndex].currentLevel++;
        PowerUpInfos[powerUpInfoIndex].PowerUpArray[PowerUpInfos[powerUpInfoIndex].currentLevel].Activate(shipMovementHandler, playerHPHandler);
    }
    public PowerUpInfoSO GetPowerUpInfo(PowerUpTypes t)
    {

        for (int i = 0; i < PowerUpInfos.Length; i++)
        {
            if (PowerUpInfos[i].PowerUpType == t)
            {
                return PowerUpInfos[i].PowerUpArray[PowerUpInfos[i].currentLevel + 1];
            }
        }
        Debug.Log("No powerup info found for the type " + t);
        return null;
    }
    public int GetPowerUpInfoIndex(PowerUpTypes t)
    {
        for (int i = 0; i < PowerUpInfos.Length; i++)
        {
            if (PowerUpInfos[i].PowerUpType == t)
            {
                return i;
            }
        }
        Debug.Log("No powerup info found for the type " + t);
        return 0;
    }

    public List<PowerUpTypes> GetNoneMaxLevelPowerUpsList()
    {
        List<PowerUpTypes> result = new List<PowerUpTypes>();
        foreach (var item in PowerUpInfos)
        {
            if (item.currentLevel < item.PowerUpArray.Length - 1)
            {
                result.Add(item.PowerUpType);
            }
        }
        //Debug.Log("Number of None Max Level Powerups: " + result);
        return result;
    }
}

