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
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Agent.Value.transform.rotation = Quaternion.Lerp(Agent.Value.transform.rotation, Quaternion.LookRotation((Player.Value.transform.position - Agent.Value.transform.position).normalized, Vector2.up), Time.deltaTime * RotSpeed.Value);
        if (Mathf.Abs(Vector3.Dot(Agent.Value.transform.forward, (Agent.Value.transform.position - Player.Value.transform.position).normalized)) < CanonAccuracy.Value)
            return Status.Running;

        Agent.Value.FireAt(Player.Value.transform.position);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

