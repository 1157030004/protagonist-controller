using System;
using System.Collections;
using System.Collections.Generic;
using Shadee.ProtagonistController.Data;
using UnityEngine;

namespace Shadee.ProtagonistController.Utilities
{
    [Serializable]
    public class ProtagonistCapsuleColliderUtility : CapsuleColliderUtility
    {
        [field: SerializeField] public ProtagonistTriggerColliderData TriggerColliderData { get; private set; }
    }
}
