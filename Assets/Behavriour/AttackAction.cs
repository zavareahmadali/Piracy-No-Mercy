using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "[Agent] Attack the [Player] with Rotationspeed of [RotationSpeed] with Accuracy of [Accuracy]", category: "Action", id: "a6ffec2cf69582f5a0fcd77f80a46e8c")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<AIShipBase> Agent;
    [SerializeReference] public BlackboardVariable<ShipMovementHandler> Player;
    [SerializeReference] public BlackboardVariable<float> RotationSpeed;
    [SerializeReference] public BlackboardVariable<float> Accuracy;
    protected override Status OnStart()
    {

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Agent.Value.transform.rotation = Quaternion.Lerp(Agent.Value.transform.rotation, Quaternion.LookRotation((Player.Value.transform.position - Agent.Value.transform.position).normalized, Vector2.up), Time.deltaTime * RotationSpeed.Value);
        if (Mathf.Abs(Vector3.Dot(Agent.Value.transform.forward, (Agent.Value.transform.position - Player.Value.transform.position).normalized)) < Accuracy.Value)
            return Status.Running;

        Agent.Value.FireAt(Player.Value.transform.position);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

