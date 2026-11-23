using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AIShipAttack", story: "[Agent] Attacks [Player] with RotationSpeed of [RotSpeed] and Accuracy of [CanonAccuracy]", category: "Action", id: "84760feac8b4398f47abfc8d34a01100")]
public partial class AiShipAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<AIShipBase> Agent;
    [SerializeReference] public BlackboardVariable<ShipMovementHandler> Player;
    [SerializeReference] public BlackboardVariable<float> RotSpeed;
    [SerializeReference] public BlackboardVariable<float> CanonAccuracy;
    private Rigidbody agentRB;
    protected override Status OnStart()
    {
        Agent.Value.FireAt(Player.Value.transform.position);
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
            return Status.Success;

        
        
    }

    protected override void OnEnd()
    {
    }
}

