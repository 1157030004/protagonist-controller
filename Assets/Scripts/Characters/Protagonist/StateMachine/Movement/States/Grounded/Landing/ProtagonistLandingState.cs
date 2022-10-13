using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistLandingState : ProtagonistGroundedState
    {
        public ProtagonistLandingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
        }

        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}
