using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.ProtagonistController.Data
{
    [Serializable]
    public class ProtagonistCameraRecenteringData
    {
        [field: SerializeField] [field: Range(0f, 360f)] public float MinimumAngle { get; private set; }
        [field: SerializeField] [field: Range(0f, 360f)] public float MaximumAngle { get; private set; }
        [field: SerializeField] [field: Range(-1f, 20f)] public float WaitTime { get; private set; }
        [field: SerializeField] [field: Range(-1f, 20f)] public float RecenteringTime { get; private set; }

        public bool IsWithinRange(float angle)
        {
            return angle >= MinimumAngle && angle <= MaximumAngle;
        }
    }
}
