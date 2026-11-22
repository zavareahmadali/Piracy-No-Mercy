using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlantMines", story: "[Agent] Plant Mines.", category: "Action", id: "3c9c702b9b536b44c4a7b71af78ea799")]
public partial class PlantMinesAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    private AIShipBig aiShipBig;
    protected override Status OnStart()
    {
        if (aiShipBig == null)
        {
            aiShipBig = Agent.Value.GetComponent<AIShipBig>(); 
        }
        aiShipBig.PlantMines();
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

