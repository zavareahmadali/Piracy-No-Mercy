using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(GameObject instigator, float damage, ShipMovementHandler.CanonType damageType, Vector3 hitPoint, Vector3 hitDirection);
}

