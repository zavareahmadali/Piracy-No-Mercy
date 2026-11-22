using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Loot", menuName = "Scriptable Objects/Loot")]
public class LootSO : ScriptableObject
{
    public bool isSmall;
    public List<LootData> lootDataList;
}
[Serializable]
public struct LootData
{
    public LootHandler.LootType type;
    public int amount;
}
