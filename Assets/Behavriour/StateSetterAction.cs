using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StateSetter", story: "Set the [State] based on [Agent] and [Player] based on MaxAttackRange: [MaxAttackRange] and MaxDetectionRange: [MaxDetectionRange]", category: "Action", id: "e194c33059aa5fd3e00bf84db5ba6424")]
public partial class StateSetterAction : Action
{
    [SerializeReference] public BlackboardVariable<States> State;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<ShipMovementHandler> Player;
    [SerializeReference] public BlackboardVariable<float> MaxAttackRange;
    [SerializeReference] public BlackboardVariable<float> MaxDetectionRange;
    protected override Status OnStart()
    {
        return Status.Running;

    }

    protected override Status OnUpdate()
    {

        switch (State.Value)
        {
            case States.Follow:
                if (Vector3.Distance(Agent.Value.transform.position, Player.Value.transform.position) <= MaxAttackRange.Value)
                    State.Value = States.Attack;
                break;
            case States.Attack:
                if (Vector3.Distance(Agent.Value.transform.position, Player.Value.transform.position) > MaxAttackRange.Value)
                    State.Value = States.Follow;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        //Debug.Log($"Current State: {State.Value}, Distance to Player: {Vector3.Distance(Agent.Value.transform.position, Player.Value.transform.position)}");
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

