using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [Serializable]
    public class ProtagonistSprintData
    {
        [field: SerializeField] [field: Range(1f, 3f)] public float SpeedModifier { get; private set; } = 1.7f;
        [field: SerializeField] [field: Range(0f, 5f)] public float SprintToRunTime { get; private set; } = 1f;
        [field: SerializeField] [field: Range(0f, 2f)] public float RunToWalkTime { get; private set; } = 0.5f;
        
    }
}
