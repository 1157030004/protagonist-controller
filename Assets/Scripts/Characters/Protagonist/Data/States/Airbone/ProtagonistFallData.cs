using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [Serializable]
    public class ProtagonistFallData
    {
        [field: SerializeField] [field: Range(1f, 10f)] public float FallSpeedLimit { get; private set; } = 10f;
        [field: SerializeField] [field: Range(0f, 100f)] public float MinimumDistanceTobeConsideredHardFall { get; private set; } = 3f;
    }
}
