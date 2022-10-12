using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistDashingState : ProtagonistGroundedState
    {
        private ProtagonistDashData _dashData;
        private float startTime;
        private int consecutiveDashesUsed;
        public ProtagonistDashingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
            _dashData = movementData.DashData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = _dashData.SpeedModifier;

            AddForceOnTransitionFromStationaryState();

            UpdateConsecutiveDashes();

            startTime = Time.time;
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
        private void AddForceOnTransitionFromStationaryState()
        {
            if(stateMachine.ReusableData.MovementInput != Vector2.zero)
                return;
            
            Vector3 characterRotationDirection = stateMachine.Protagonist.transform.forward;

            characterRotationDirection.y = 0f;

            stateMachine.Protagonist.Rigidbody.velocity = characterRotationDirection * GetMovementSpeed();
        }

        private void UpdateConsecutiveDashes()
        {
            if(!IsConsecutive())
            {
                consecutiveDashesUsed = 0;
            }

            ++consecutiveDashesUsed;

            if(consecutiveDashesUsed == _dashData.ConsecutiveDashesLimitAmount)
            {
                consecutiveDashesUsed = 0;
                stateMachine.Protagonist.Input.DisableActionFor(stateMachine.Protagonist.Input.ProtagonistActions.Dash, _dashData.DashLimitReachCooldown);
            }
        }

        private bool IsConsecutive()
        {
            return Time.time < startTime + _dashData.TimeToBeConsideredConsecutive;
        }
        #endregion

        #region Input Methods

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}
