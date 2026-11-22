using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEditor;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DetectSomething", story: "Detect [Something] in [Range] around [Agent]", category: "Action", id: "f4d0dfa172442c4bedb76dca2d4a22db")]
public partial class DetectSomethingAction : Action
{
    [SerializeReference] public BlackboardVariable<ShipMovementHandler> Something;
    [SerializeReference] public BlackboardVariable<float> Range;
    [SerializeReference] public BlackboardVariable<Transform> Agent;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
          
        if(Physics.CheckSphere(Agent.Value.position, Range.Value,LayerMask.GetMask("Player")))
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

