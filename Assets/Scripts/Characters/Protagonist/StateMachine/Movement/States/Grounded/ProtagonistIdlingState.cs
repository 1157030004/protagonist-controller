using System;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistIdlingState : ProtagonistGroundedState
    {
        public ProtagonistIdlingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            ResetVelocity();
        }

        public override void Update()
        {
            base.Update();

            if(stateMachine.ReusableData.MovementInput == Vector2.zero)
                return;

            OnMove();
        }
        #endregion
    }
}
