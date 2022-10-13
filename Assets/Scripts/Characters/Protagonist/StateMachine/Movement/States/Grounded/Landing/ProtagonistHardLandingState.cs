using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistHardLandingState : ProtagonistLandingState
    {
        public ProtagonistHardLandingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.Protagonist.Input.ProtagonistActions.Movement.Disable();

            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            stateMachine.Protagonist.Input.ProtagonistActions.Movement.Enable();
        }

        public override void OnAnimationExitEvent()
        {
            stateMachine.Protagonist.Input.ProtagonistActions.Movement.Enable();           
        }

        public override void OnAnimationTransitionEvent()
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();
            stateMachine.Protagonist.Input.ProtagonistActions.Movement.started += OnMovementStarted;
        }

        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            stateMachine.Protagonist.Input.ProtagonistActions.Movement.started -= OnMovementStarted;
        }
        protected override void OnMove()
        {
            if(stateMachine.ReusableData.ShouldWalk)
                return;
            
            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion

        #region Input Methods
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }
        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        #endregion
    }
}
