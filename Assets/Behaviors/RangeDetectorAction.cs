using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RangeDetector", story: "Update Range [Detector] and assing [Target]", category: "Action",
    id: "1e8d28620b6d315a62d6644c2147a7ad")]
public partial class RangeDetectorAction : Action
{
    [SerializeReference] public BlackboardVariable<RangeDetector> Detector;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<bool> IsInAttackRange;

    // protected override Status OnStart()
    // {
    //     return Status.Running;
    // }

    protected override Status OnUpdate()
    {
        if (Detector.Value == null || Target.Value == null)
        {
            IsInAttackRange.Value = false;
            return Status.Failure;
        }

        bool inRange = Detector.Value.IsAnyTargetInAttackRange(out _);
        IsInAttackRange.Value = inRange;

        return inRange ? Status.Success : Status.Failure;
    }

    // protected override void OnEnd()
    // {
    // }
}