using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class LootHandler : MonoBehaviour
{
 
    public enum LootType
    {
        Gold,
        XP,
        Item,
        HP,
        NormalCanonBall,
        FireCanonBall,
        ShotgunCanonBall,
        IceCanonBall,
    }
    [SerializeField] private LootTier lootTier;
    [SerializeField] private List<LootData> lootData;
    
    internal Animator animator;
    private GameObject VFX;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void DestroyThis()
    {

        Destroy(gameObject, 2f);
    }

    public virtual void GetLooted()
    {
        
        FindAnyObjectByType<PlayerLootHandler>().GetLoot(lootData);
        GetComponent<Collider>().enabled = false;

        if (TryGetComponent<AudioHandler>(out AudioHandler audioHandler))
            audioHandler.PlaySFX(SFXType.Loot);
        
        DestroyThis();
    }

    public void Initialize(List<LootData> lootData, LootTier tier, GameObject vfx)
    {
        this.lootData = lootData;
        lootTier = tier;
        this.VFX = vfx;
        VFX = Instantiate(vfx, transform.position, vfx.transform.rotation);
        VFX.transform.localScale = GetComponent<SphereCollider>().radius * Vector3.one;
        VFX.transform.parent = this.transform;
    }
    
    public virtual void LootAreaEntered() { }

    public virtual void LootAreaExited() { }
}
