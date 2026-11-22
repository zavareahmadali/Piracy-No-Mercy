using System;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public GameObject SmallLootPrefab;
    
    public GameObject BigLootPrefab;
    
    public static LootManager Instance;
    
    [SerializeField]
    public Dictionary<LootTier, List<LootItem>> lootDictionary;
    [SerializeField]
    public Dictionary<LootTier, GameObject> lootVFXDictionary;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void GiveLoot(Vector3 pos, List<LootChanceTable> chances)
    {
        float rand = UnityEngine.Random.Range(0, 101);
        LootTier resultingTier = LootTier.Common;
        foreach (var item in chances)
        {
            if (rand <= item.chance)
            {
                resultingTier = item.tier;
                break;
            }
        }
        LootItem lootitem = Instance.lootDictionary[resultingTier][UnityEngine.Random.Range(0, Instance.lootDictionary[resultingTier].Count)];
        bool isSmall = lootitem.lootInfo.isSmall;

        
        LootHandler loot = Instantiate(isSmall ? Instance.SmallLootPrefab : Instance.BigLootPrefab,
            pos, 
            isSmall ? Instance.SmallLootPrefab.transform.rotation : Instance.BigLootPrefab.transform.rotation)
            .GetComponent<LootHandler>();

        loot.transform.position = new Vector3(pos.x, isSmall? Instance.SmallLootPrefab.transform.position.y: Instance.BigLootPrefab.transform.position.y, pos.z);
        loot.Initialize(lootitem.lootInfo.lootDataList, resultingTier, Instance.lootVFXDictionary[resultingTier]);
        //Debug.Log("Random Found Tier is: " + resultingTier +  "-" + lootitem.lootInfo.name);
    }
}
[Serializable]
public struct LootItem
{
    public LootSO lootInfo;
}
public enum LootTier
{
    Common,
    Epic,
    Legendary
}

[Serializable]
public struct LootChanceTable
{
    public LootTier tier;
    public float chance;
}
