using System;
using System.Collections.Generic;
using Shadee.ProtagonistController.Data;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [Serializable]
    public class ProtagonistIdleData
    {
        [field: SerializeField] public List<ProtagonistCameraRecenteringData> BackwardsCameraRecenteringData { get; private set;}
    }
}
