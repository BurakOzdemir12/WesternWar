using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindTarget", story: "Find [Target] By [RangeDetector]", category: "Action",
    id: "6441e1e17d88f119e50fa041a26a4858")]
public partial class FindTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<RangeDetector> RangeDetector;
    [SerializeReference] public BlackboardVariable<bool> IsInChaseRange;

    // protected override Status OnStart()
    // {
    //     return Status.Running;
    // }

    protected override Status OnUpdate()
    {
        var det = RangeDetector.Value;
        if (det == null) return Status.Failure;

        if (det.IsAnyTargetInRange(out var go))
            Target.Value = go;   
        IsInChaseRange.Value = Target.Value != null;

        return Target.Value != null ? Status.Success : Status.Failure;
    }

    // protected override void OnEnd()
    // {
    // }
}