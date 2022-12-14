using System;
using Shadee.ProtagonistController.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistGroundedState : ProtagonistMovementState
    {
        private SlopeData _slopeData;
        public ProtagonistGroundedState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
            _slopeData = stateMachine.Protagonist.ColliderUtility.SlopeData;
        }

        #region IState Methods

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Protagonist.AnimationData.GroundedParameterHash);

            UpdateShouldSpritnState();

            UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Protagonist.AnimationData.GroundedParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Float();
        }
        #endregion

        #region Main Methods

        private void UpdateShouldSpritnState()
        {
            if(!stateMachine.ReusableData.ShouldSprint)
                return;
            
            if(stateMachine.ReusableData.MovementInput != Vector2.zero)
                return;

            stateMachine.ReusableData.ShouldSprint = false;
        }
        private void Float()
        {
            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Protagonist.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if(Physics.Raycast(
                downwardsRayFromCapsuleCenter,
                out RaycastHit hit, 
                _slopeData.FloatRayDistance, 
                stateMachine.Protagonist.LayerData.GroundLayer, 
                QueryTriggerInteraction.Ignore))
                {
                    float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                    float slopeSpeedModifier =  SetSlopeSpeedModifierOnAngle(groundAngle);

                    if(slopeSpeedModifier == 0f) return;

                    float distanceToFloatingPoint = stateMachine.Protagonist.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y
                     * stateMachine.Protagonist.transform.localScale.y
                     - hit.distance;

                    if(distanceToFloatingPoint == 0f) return;

                    float amountToLift = distanceToFloatingPoint * _slopeData.StepReachForce - GetProtagonistVerticalVelocity().y;

                    Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

                    stateMachine.Protagonist.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
                }
        }

        private float SetSlopeSpeedModifierOnAngle(float angle)
        {
            float slopeSpeedModifier = movementData.SlopeSpeedAngles.Evaluate(angle);

            if(stateMachine.ReusableData.MovementOnSlopesSpeedModifier != slopeSpeedModifier)
            {
                stateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;

                UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);
            }

            stateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;

            return slopeSpeedModifier;
        }
       
        private bool IsThereGroundUnderneath()
        {
            BoxCollider groundCheckCollider = stateMachine.Protagonist.ColliderUtility.TriggerColliderData.GroundCheckCollider;
            Vector3 groundColliderCenterInWorldSpace = groundCheckCollider.bounds.center;

            Collider[] overlappedGroundColliders = Physics.OverlapBox(
                groundColliderCenterInWorldSpace, 
                stateMachine.Protagonist.ColliderUtility.TriggerColliderData.GroundCheckColliderExtents, 
                groundCheckCollider.transform.rotation, 
                stateMachine.Protagonist.LayerData.GroundLayer, 
                QueryTriggerInteraction.Ignore);

            return overlappedGroundColliders.Length > 0;
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();

            stateMachine.Protagonist.Input.ProtagonistActions.Dash.started += OnDashStarted;

            stateMachine.Protagonist.Input.ProtagonistActions.Jump.started += OnJumpStarted;
        }

        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();

            stateMachine.Protagonist.Input.ProtagonistActions.Dash.started -= OnDashStarted;

            stateMachine.Protagonist.Input.ProtagonistActions.Jump.started -= OnJumpStarted;
        }
        
        protected virtual void OnMove()
        {
            if(stateMachine.ReusableData.ShouldSprint)
            {
                stateMachine.ChangeState(stateMachine.SprintingState);
                return;
            }
            if(stateMachine.ReusableData.ShouldWalk)
            {
                stateMachine.ChangeState(stateMachine.WalkingState);
                return;
            }

            stateMachine.ChangeState(stateMachine.RunningState);
        }

        protected override void OnContactWithGroundExited(Collider collider)
        {
            base.OnContactWithGroundExited(collider);

            if(IsThereGroundUnderneath())
                return;


            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Protagonist.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleBottom = new Ray(
                capsuleColliderCenterInWorldSpace - stateMachine.Protagonist.ColliderUtility.CapsuleColliderData.ColliderVerticalExtents,
                Vector3.down);

            if(!Physics.Raycast(
                downwardsRayFromCapsuleBottom,
                out _, 
                movementData.GroundToFallRayDistance, 
                stateMachine.Protagonist.LayerData.GroundLayer, 
                QueryTriggerInteraction.Ignore))
            {
                OnFall();
            }
        }

        protected virtual void OnFall()
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }
        #endregion

        #region Input Methods
        
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            stateMachine.ChangeState(stateMachine.RunningState);
        }

        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.DashingState);
        }

        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.JumpingState);
        }
        
        protected override void OnMovementPerformed(InputAction.CallbackContext context)
        {
            base.OnMovementPerformed(context);
            UpdateTargetRotation(GetMovementInputDirection());
        }
        #endregion
    }
}
