using UnityEngine;

[CreateAssetMenu(fileName = "HPUpPowerUp", menuName = "Scriptable Objects/HPUpPowerUp")]
public class HPUpPowerUp : PowerUpInfoSO
{
    public int hpIncreaseAmount;
    public override void Activate(ShipMovementHandler shipMovementHandler, PlayerHPHandler playerHPHandler)
    {
        playerHPHandler.ChangeMaxHP(playerHPHandler.maxHP +  hpIncreaseAmount);
    }
}

