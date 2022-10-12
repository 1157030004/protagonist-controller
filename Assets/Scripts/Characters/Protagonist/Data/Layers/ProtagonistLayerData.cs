using System;
using UnityEngine;

namespace Shadee.ProtagonistController.Data
{
    [Serializable]
    public class ProtagonistLayerData
    {
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    }
}
