using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistFallingState : ProtagonistAirboneState
    {
        private ProtagonistFallData fallData;
        private Vector3 protagonistPositionOnEnter;
        public ProtagonistFallingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
            fallData = airboneData.FallData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            protagonistPositionOnEnter = stateMachine.Protagonist.transform.position;

            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            ResetVerticalVelocity();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            LimitVerticalVelocity();
        }
        #endregion

        #region Reusable Methods
        protected override void ResetSprintState()
        {
        }

        protected override void OnContactWithGround(Collider collider)
        {
            float fallDistance = Mathf.Abs(protagonistPositionOnEnter.y - stateMachine.Protagonist.transform.position.y);

            if(fallDistance < fallData.MinimumDistanceTobeConsideredHardFall)
            {
                stateMachine.ChangeState(stateMachine.LightLandingState);
                return;
            }

            if(stateMachine.ReusableData.ShouldWalk && !stateMachine.ReusableData.ShouldSprint || stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.HardLandingState);
                return;
            }

            stateMachine.ChangeState(stateMachine.RollingState);
        }
        #endregion
    
        #region Main Methods
        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetProtagonistVerticalVelocity();
            if(playerVerticalVelocity.y >= -fallData.FallSpeedLimit)
                return;

            Vector3 limitedVelocity = new Vector3(0f, -fallData.FallSpeedLimit - playerVerticalVelocity.y, 0f);

            stateMachine.Protagonist.Rigidbody.AddForce(limitedVelocity, ForceMode.VelocityChange);
        }
        #endregion
    }
}
