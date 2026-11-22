using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLootHandler : MonoBehaviour
{
    public int goldAmount;
    [SerializeField] public Dictionary<ShipMovementHandler.CanonType, int> canonBallInventory;
    public static Action<int> OnGoldAdded;
    public IndicatorHandler indicatorHandler;
    public Transform LootSpawnPosition;
    public static PlayerLootHandler instance;
    private ShipCanonHandler[] shipCanonHandlers;
    public static Action<ShipMovementHandler.CanonType, int> OnUpdateShells;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        shipCanonHandlers = FindObjectsByType<ShipCanonHandler>();
        foreach (var c in shipCanonHandlers)
        {
            c.OnShootAction += OnShellUsed;
        }
        OnUpdateShells?.Invoke(ShipMovementHandler.currentCanonType, canonBallInventory[ShipMovementHandler.currentCanonType]);
    }
    private void OnShellUsed(ShipMovementHandler.CanonType canonType, int amount, float recoilForce, Vector3 recoilDirection)
    {
        if (canonBallInventory.TryGetValue(canonType, out int count))
        {
            if (count >= amount)
            {
                canonBallInventory[canonType] -= amount;
                OnUpdateShells?.Invoke(canonType, canonBallInventory[canonType]);
            }
            else
            {
                Debug.LogWarning($"Not enough shells of type {canonType}. Requested: {amount}, Available: {count}");
            }
        }
        else
        {
            Debug.LogWarning($"No inventory entry for shell type {canonType}");
        }
    }
    public void GetLoot(List<LootData> lootData)
    {
        Vector3 tempPos;
        for (int i = 0; i < lootData.Count; i++)
        {
            var loot = lootData[i];
            tempPos = LootSpawnPosition.position + Vector3.up * i * 4f;
            switch (loot.type)
            {
                case LootHandler.LootType.Gold:
                    goldAmount += loot.amount;
                    OnGoldAdded?.Invoke(loot.amount);
                    break;

                case LootHandler.LootType.XP:
                    XPHandler.GetXP(loot.amount);
                    break;

                case LootHandler.LootType.HP:
                    PlayerHPHandler.Heal(loot.amount);
                    break;

                case LootHandler.LootType.Item:
                    // Handle item loot
                    break;

                case LootHandler.LootType.NormalCanonBall:
                    canonBallInventory[ShipMovementHandler.CanonType.Normal] += loot.amount;
                    break;

                case LootHandler.LootType.FireCanonBall:
                    canonBallInventory[ShipMovementHandler.CanonType.Fire] += loot.amount;
                    break;

                case LootHandler.LootType.ShotgunCanonBall:
                    canonBallInventory[ShipMovementHandler.CanonType.Shotgun] += loot.amount;
                    break;

                case LootHandler.LootType.IceCanonBall:
                    canonBallInventory[ShipMovementHandler.CanonType.Ice] += loot.amount;
                    break;

            }
            Instantiate(indicatorHandler, tempPos, Quaternion.identity).Initialize(loot.type, loot.amount.ToString());
        }

    }
    public static int GetShellCount(ShipMovementHandler.CanonType canonType)
    {
        if (instance.canonBallInventory.TryGetValue(canonType, out int count))
            return count;

        return 0;
    }
    public static void AddGold(int amount)
    {
        instance.goldAmount += amount;
        OnGoldAdded?.Invoke(amount);
    }
}
