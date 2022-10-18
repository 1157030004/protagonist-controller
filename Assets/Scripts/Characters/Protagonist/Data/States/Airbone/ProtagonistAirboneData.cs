using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [Serializable]
    public class ProtagonistAirboneData
    {
        [field: SerializeField] public ProtagonistJumpData JumpData { get; private set; }
        [field: SerializeField] public ProtagonistFallData FallData { get; private set; }
        [field: SerializeField] public ProtagonistGlideData GlideData { get; private set; }
    }
}
