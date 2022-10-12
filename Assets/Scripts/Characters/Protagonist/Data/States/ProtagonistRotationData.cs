using System;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [Serializable]
    public class ProtagonistRotationData
    {
        [field: SerializeField] public Vector3 TargetRotationReachTime { get; private set; }
    }
}
