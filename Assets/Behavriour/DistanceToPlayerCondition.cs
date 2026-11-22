using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "DistanceToPlayer", story: "Distance from [Agent] to [Player] [GreaterThan] [Amount]", category: "Conditions", id: "9b1e10492ff52319e3b9679badf02a4e")]
public partial class DistanceToPlayerCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Transform> Agent;
    [SerializeReference] public BlackboardVariable<ShipMovementHandler> Player;
    [SerializeReference] public BlackboardVariable<float> Amount;
    [SerializeReference] public BlackboardVariable<bool> GreaterThan;
    public override bool IsTrue()
    {
        if (GreaterThan.Value)
        {
            return Vector3.Distance(Agent.Value.position, Player.Value.transform.position) > Amount.Value;
        }
        else
        {
            return Vector3.Distance(Agent.Value.position, Player.Value.transform.position) < Amount.Value;
        }
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
