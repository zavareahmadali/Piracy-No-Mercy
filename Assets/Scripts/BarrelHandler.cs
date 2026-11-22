using UnityEngine;

public class BarrelHandler : MonoBehaviour, IDamagable
{
    
    public float maxHP = 1;
    private float hp = 1;
    void Start()
    {
        hp = maxHP;
    }

    void IDamagable.TakeDamage(GameObject instigator, float damage, ShipMovementHandler.CanonType damageType, Vector3 hitPoint, Vector3 hitDirection)
    {
        hp -= damage;
        hp = Mathf.Clamp(hp, 0, maxHP);

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

}
