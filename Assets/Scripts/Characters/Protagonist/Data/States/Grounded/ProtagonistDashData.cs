using System;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [Serializable]
    public class ProtagonistDashData
    {
        [field: SerializeField] [field: Range(1f, 3f)] public float SpeedModifier { get; private set; } = 2f;
        [field: SerializeField] [field: Range(0f, 2f)] public float TimeToBeConsideredConsecutive { get; private set; } = 1f;
        [field: SerializeField] [field: Range(1, 10)] public int ConsecutiveDashesLimitAmount { get; private set; } = 2;
        [field: SerializeField] [field: Range(0f, 5f)] public float DashLimitReachCooldown { get; private set; } = 1.75f;
    }
}
