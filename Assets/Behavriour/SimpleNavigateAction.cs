using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SimpleNavigate", story: "Navigate [Target1] to [Target2] and stop at [Distance]", category: "Action", id: "ba11120f9acb694e59528a73088eb327")]
public partial class SimpleNavigateAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target1;
    [SerializeReference] public BlackboardVariable<GameObject> Target2;
    [SerializeReference] public BlackboardVariable<float> Distance;
    private NavMeshAgent target1NavMeshAgent;
    protected override Status OnStart()
    {
        target1NavMeshAgent = Target1.Value.GetComponent<NavMeshAgent>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        target1NavMeshAgent.SetDestination(Target2.Value.transform.position);

        if (Vector3.Distance(Target1.Value.transform.position, Target2.Value.transform.position) > Distance.Value)
        {
            return Status.Running;
        }
        else
        {
            return Status.Success;
        }
        
    }

    protected override void OnEnd()
    {
    }
}

