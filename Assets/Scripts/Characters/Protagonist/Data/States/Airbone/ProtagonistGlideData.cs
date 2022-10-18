using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [Serializable]
    public class ProtagonistGlideData
    {
        [field: SerializeField] public Vector3 GlideForce { get; private set; }
    }
}
