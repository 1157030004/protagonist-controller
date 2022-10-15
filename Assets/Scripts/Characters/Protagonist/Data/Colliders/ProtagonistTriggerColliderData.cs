using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.ProtagonistController.Data
{
    [Serializable]
    public class ProtagonistTriggerColliderData
    {
        [field: SerializeField] public BoxCollider GroundCheckCollider { get; private set; }

        public Vector3 GroundCheckColliderExtents { get; private set; }

        public void Initialize()
        {
            GroundCheckColliderExtents = GroundCheckCollider.bounds.extents;
        }
    }
}
