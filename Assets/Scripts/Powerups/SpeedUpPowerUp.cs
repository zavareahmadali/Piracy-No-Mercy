using UnityEngine;

[CreateAssetMenu(fileName = "SpeedUpPowerUp", menuName = "Scriptable Objects/SpeedUpPowerUp")]
public class SpeedUpPowerUp : PowerUpInfoSO
{
    public float speed = 5f;
    public float rotationSpeed = 1f;
    public float minimumCollisionSpeed = 5f;
    public float maxCollisionSpeed = 40f;
    
    public override void Activate(ShipMovementHandler shipMovementHandler, PlayerHPHandler playerHPHandler)
    {
        shipMovementHandler.speed = speed;
        shipMovementHandler.rotationSpeed = rotationSpeed;
        shipMovementHandler.minimumCollisionSpeed = minimumCollisionSpeed;
        shipMovementHandler.maxCollisionSpeed = maxCollisionSpeed;
    }
}

