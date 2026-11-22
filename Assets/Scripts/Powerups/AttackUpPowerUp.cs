using UnityEngine;

[CreateAssetMenu(fileName = "AttackUpPowerUp", menuName = "Scriptable Objects/AttackUpPowerUp")]
public class AttackUpPowerUp : PowerUpInfoSO
{
    public float increasedAttackPower;
    public override void Activate(ShipMovementHandler shipMovementHandler, PlayerHPHandler playerHPHandler)
    {
        ShipMovementHandler.ChangeAttackMultiplier(1 + increasedAttackPower);
    }
}

