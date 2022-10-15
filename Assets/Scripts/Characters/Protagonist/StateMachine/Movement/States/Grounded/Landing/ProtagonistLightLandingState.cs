using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistLightLandingState : ProtagonistLandingState
    {
        public ProtagonistLightLandingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = 0f;
            base.Enter();

            stateMachine.ReusableData.CurrentJumpForce = airboneData.JumpData.StationaryForce;
            ResetVelocity();
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

        public override void OnAnimationTransitionEvent()
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        #endregion
    }
}
