using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Follow", story: "[Agent1] Follow [Agent2]", category: "Action", id: "37c81adab7aa63289b47afa4f17201c7")]
public partial class FollowAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent1;
    [SerializeReference] public BlackboardVariable<GameObject> Agent2;

    private AIShipBase agent1MovementHandler;
    protected override Status OnStart()
    {
        agent1MovementHandler = Agent1.Value.GetComponent<AIShipBase>();
        agent1MovementHandler.MoveTo(Agent2.Value.transform.position);
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        //Debug.Log("Has Reached Destination: " + agent1MovementHandler.HasReachedDestination());
        if (!agent1MovementHandler.HasReachedDestination())
            return Status.Running;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

