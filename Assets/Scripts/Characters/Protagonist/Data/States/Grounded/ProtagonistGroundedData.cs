using System;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [Serializable]
    public class ProtagonistGroundedData
    {
        [field: SerializeField] [field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
        [field: SerializeField] [field: Range(0f, 5f)] public float GroundToFallRayDistance { get; private set; } = 1f;
        [field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }
        [field: SerializeField] public ProtagonistRotationData BaseRotationData { get; private set; }
        [field: SerializeField] public ProtagonistWalkData WalkData { get; private set; }
        [field: SerializeField] public ProtagonistRunData RunData { get; private set; }
        [field: SerializeField] public ProtagonistSprintData SprintData { get; private set; }
        [field: SerializeField] public ProtagonistDashData DashData { get; private set; }
        [field: SerializeField] public ProtagonistStopData StopData { get; private set; }   
        [field: SerializeField] public ProtagonistRollData RollData { get; private set; }   
    }
}
