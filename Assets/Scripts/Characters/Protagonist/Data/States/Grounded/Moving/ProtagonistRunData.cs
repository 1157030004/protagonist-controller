using System;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [Serializable]
    public class ProtagonistRunData
    {
        [field: SerializeField] [field: Range(1f, 2f)] public float SpeedModifier { get; private set;} = 1f;
    }
}
