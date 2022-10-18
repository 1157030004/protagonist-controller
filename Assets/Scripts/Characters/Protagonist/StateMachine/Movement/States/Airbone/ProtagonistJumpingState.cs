using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistJumpingState : ProtagonistAirboneState
    {
        private bool shouldKeepRotating;
        private ProtagonistJumpData jumpData;
        private bool canStartFalling;
        public ProtagonistJumpingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
            jumpData = airboneData.JumpData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = 0f;
            stateMachine.ReusableData.RotationData = jumpData.RotationData;
            stateMachine.ReusableData.MovementDecelerationForce = jumpData.DecelerationForce;

            shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

            Jump();
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();

            canStartFalling = false;
        }

        public override void Update()
        {
            base.Update();

            if(!canStartFalling && IsMovingUp(0f))
            {
                canStartFalling = true;
            }

            if(!canStartFalling || GetProtagonistVerticalVelocity().y > 0)
                return;

            stateMachine.ChangeState(stateMachine.FallingState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if(shouldKeepRotating)
            {
                RotateTowardsTargetRotation();
            }

            if(IsMovingUp())
            {
                DecelerateVertically();
            }
        }
        #endregion

        #region Reusable Methods
        protected override void ResetSprintState()
        {
        }
        #endregion

        #region Main Methods
        private void Jump()
        {
            Vector3 jumpForce = stateMachine.ReusableData.CurrentJumpForce;

            Vector3 jumpDirection = stateMachine.Protagonist.transform.forward;

            if(shouldKeepRotating)
            {
                UpdateTargetRotation(GetMovementInputDirection());
                jumpDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
            }

            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;

            Vector3 cappsuleColliderCenterInWorldSpace = stateMachine.Protagonist.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(cappsuleColliderCenterInWorldSpace, Vector3.down);

            if(Physics.Raycast(
                downwardsRayFromCapsuleCenter,
                out RaycastHit hit, 
                jumpData.JumpToGroundRayDistance, 
                stateMachine.Protagonist.LayerData.GroundLayer, 
                QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                if(IsMovingUp())
                {
                    float forceModifier = jumpData.JumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                    jumpForce.x *= forceModifier;
                    jumpForce.z *= forceModifier;
                }

                if(IsMovingDown())
                {
                    float forceModifier = jumpData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                    jumpForce.y *= forceModifier;
                }
            }

            ResetVelocity();

            stateMachine.Protagonist.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
        }

        #endregion

        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
        
        #endregion
    }
}