using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TimeElapsed", story: "[Duration] Elapsed", category: "Action", id: "9b497cfa1fb2ab3f2e98362cc8115f92")]
public partial class TimeElapsedAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Duration;
    float timer = 0f;
    protected override Status OnStart()
    {
        timer = 0f;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= Duration.Value)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

