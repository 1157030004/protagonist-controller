using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [Serializable]
    public class ProtagonistRollData
    {
        [field: SerializeField] [field: Range(0f, 3f)] public float SpeedModifier { get; private set; } = 1f;
    }
}
