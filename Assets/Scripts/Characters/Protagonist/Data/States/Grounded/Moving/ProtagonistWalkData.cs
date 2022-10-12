using System;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [Serializable]
    public class ProtagonistWalkData
    {
        [field: SerializeField] [field: Range(0f, 1f)] public float SpeedModifier { get; private set;} = 0.225f;

    }
}
