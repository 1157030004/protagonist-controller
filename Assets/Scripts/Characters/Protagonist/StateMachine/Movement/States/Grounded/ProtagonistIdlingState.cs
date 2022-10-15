using System;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistIdlingState : ProtagonistGroundedState
    {
        private ProtagonistIdleData idleData;
        public ProtagonistIdlingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
            idleData = movementData.IdleData;
        }

        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = 0f;
            stateMachine.ReusableData.BackwardsCameraRecenteringData = idleData.BackwardsCameraRecenteringData;
            base.Enter();

            StartAnimation(stateMachine.Protagonist.AnimationData.IdleParameterHash);

            stateMachine.ReusableData.CurrentJumpForce = airboneData.JumpData.StationaryForce;

            ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Protagonist.AnimationData.IdleParameterHash);
        }

        public override void Update()
        {
            base.Update();

            if(stateMachine.ReusableData.MovementInput == Vector2.zero)
                return;

            OnMove();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if(!IsMovingHorizontally())
                return;
            
            ResetVelocity();
        }
        #endregion
    }
}
