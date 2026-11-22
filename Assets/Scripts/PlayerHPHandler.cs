using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHPHandler : MonoBehaviour
{
    [SerializeField] internal float maxHP = 100f;
    [SerializeField] private DamageIndicator damageIndicator;
    [SerializeField] private Transform damageIndicatorPosition;
    
    private float CurrentHP;
    
    public static Action<float, float> UpdateHP;
    public static Action<float, float> MaxHPUpdated;
    public static Action OnDie;
    public static PlayerHPHandler instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CurrentHP = maxHP;
        UpdateHP?.Invoke(CurrentHP, maxHP);
    }
    
    public void GetDamage(float damage)
    {
        CurrentHP -= damage;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, maxHP);
        Instantiate(damageIndicator, damageIndicatorPosition.position, Quaternion.identity).Show(damage.ToString("F1"));
        if (CurrentHP == 0)
        {
            OnDie?.Invoke();
        }
        UpdateHP?.Invoke(CurrentHP, maxHP);
    }

    public static void Heal(float amount)
    {
        instance.CurrentHP += amount;
        instance.CurrentHP = Mathf.Clamp(instance.CurrentHP, 0, instance.maxHP);
        UpdateHP?.Invoke(instance.CurrentHP, instance.maxHP);
    }
    
    public void ChangeMaxHP(float maxHP)
    {
        MaxHPUpdated?.Invoke(maxHP, this.maxHP);
        this.maxHP = maxHP;
        CurrentHP = this.maxHP;
        UpdateHP?.Invoke(CurrentHP, this.maxHP);

    }
}
