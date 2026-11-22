using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetState", story: "Set [States] to [State]", category: "Action", id: "252cb232c77536859b65dd98e72f1f84")]
public partial class SetStateAction : Action
{
    [SerializeReference] public BlackboardVariable<States> States;
    [SerializeReference] public BlackboardVariable<States> State;
    protected override Status OnStart()
    {
        States.Value = State.Value;
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

