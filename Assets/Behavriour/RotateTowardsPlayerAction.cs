using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RotateTowardsPlayer", story: "Rotate [Agent] Towards [Player] with speed of [RSpeed] with accuracy of [CanonAccuracy]", category: "Action", id: "f8073360377c9b62a312d470fb91173f")]
public partial class RotateTowardsPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<Rigidbody> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> RSpeed;
    [SerializeReference] public BlackboardVariable<float> CanonAccuracy;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Agent.Value.MoveRotation(Quaternion.RotateTowards(Agent.Value.rotation, Quaternion.LookRotation((Player.Value.transform.position - Agent.Value.transform.position).normalized, Vector2.up), RSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(Vector3.Dot(Agent.Value.transform.forward, (Agent.Value.transform.position - Player.Value.transform.position).normalized)) < CanonAccuracy.Value)
            return Status.Running;

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

