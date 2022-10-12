using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistGroundedState : ProtagonistMovementState
    {
        public ProtagonistGroundedState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
        }

        #region Reusable Methods
        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();

            stateMachine.Protagonist.Input.ProtagonistActions.Movement.canceled += OnMovementCanceled;
        }

        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();

            stateMachine.Protagonist.Input.ProtagonistActions.Movement.canceled -= OnMovementCanceled;
        }
        
        protected virtual void OnMove()
        {
            if(stateMachine.ReusableData.ShouldWalk)
            {
                stateMachine.ChangeState(stateMachine.WalkingState);
                return;
            }

            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion

        #region Input Methods
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            stateMachine.ChangeState(stateMachine.RunningState);
        }

        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        #endregion
    }
}
