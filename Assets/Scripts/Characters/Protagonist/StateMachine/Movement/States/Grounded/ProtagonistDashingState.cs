using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistDashingState : ProtagonistGroundedState
    {
        private ProtagonistDashData dashData;
        private float startTime;
        private int consecutiveDashesUsed;
        private bool shouldKeepRotating;
        public ProtagonistDashingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
            dashData = movementData.DashData;
        }

        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = dashData.SpeedModifier;
            base.Enter();

            StartAnimation(stateMachine.Protagonist.AnimationData.DashParameterHash);

            stateMachine.ReusableData.CurrentJumpForce = airboneData.JumpData.StrongForce;
            stateMachine.ReusableData.RotationData = dashData.RotationData;

            Dash();

            shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

            UpdateConsecutiveDashes();

            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Protagonist.AnimationData.DashParameterHash);

            SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if(!shouldKeepRotating)
                return;

            RotateTowardsTargetRotation();
        }

        public override void OnAnimationTransitionEvent()
        {
            if(stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.HardStoppingState);
                return;
            }

            stateMachine.ChangeState(stateMachine.SprintingState);
        }
        #endregion

        #region Main Methods
        private void Dash()
        {
            Vector3 dashDirection = stateMachine.Protagonist.transform.forward;

            dashDirection.y = 0f;

            UpdateTargetRotation(dashDirection, false);
            
            if(stateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                UpdateTargetRotation(GetMovementInputDirection());
                dashDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
            }

            stateMachine.Protagonist.Rigidbody.velocity = dashDirection * GetMovementSpeed(false);
        }

        private void UpdateConsecutiveDashes()
        {
            if(!IsConsecutive())
            {
                consecutiveDashesUsed = 0;
            }

            ++consecutiveDashesUsed;

            if(consecutiveDashesUsed == dashData.ConsecutiveDashesLimitAmount)
            {
                consecutiveDashesUsed = 0;
                stateMachine.Protagonist.Input.DisableActionFor(stateMachine.Protagonist.Input.ProtagonistActions.Dash, dashData.DashLimitReachCooldown);
            }
        }

        private bool IsConsecutive()
        {
            return Time.time < startTime + dashData.TimeToBeConsideredConsecutive;
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();

            stateMachine.Protagonist.Input.ProtagonistActions.Movement.performed += OnMovementPerformed;
        }

        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();

            stateMachine.Protagonist.Input.ProtagonistActions.Movement.performed -= OnMovementPerformed;
        }
        #endregion

        #region Input Methods

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            shouldKeepRotating = true;
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}
