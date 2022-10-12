using System;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [Serializable]
    public class ProtagonistGroundedData
    {
        [field: SerializeField] [field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
        [field: SerializeField] public ProtagonistRotationData BaseRotationData { get; private set; }
        [field: SerializeField] public ProtagonistWalkData WalkData { get; private set; }
        [field: SerializeField] public ProtagonistRunData RunData { get; private set; }
    }
}
