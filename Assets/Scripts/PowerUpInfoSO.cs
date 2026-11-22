using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpInfoSO", menuName = "Scriptable Objects/PowerUpInfoSO")]
public class PowerUpInfoSO : ScriptableObject
{
    public PowerUpTypes powerUpType;
    public string title;
    public Sprite icon;
    public int level;
    [TextArea]
    public string description;

    public virtual void Activate(ShipMovementHandler shipMovementHandler, PlayerHPHandler playerHPHandler ) { }
}

public enum PowerUpTypes
{
    AttackPower,
    HpIncrease,
    SpeedIncrease
}

