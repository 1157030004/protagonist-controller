using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistRollingState : ProtagonistLandingState
    {
        private ProtagonistRollData rollData;
        public ProtagonistRollingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
            rollData = movementData.RollData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = rollData.SpeedModifier;
            stateMachine.ReusableData.ShouldSprint = false;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if(stateMachine.ReusableData.MovementInput != Vector2.zero)
                return;
            
            RotateTowardsTargetRotation();
        }

        public override void OnAnimationTransitionEvent()
        {
            if(stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.MediumStoppingState);
                return;
            }

            OnMove();
        }
        #endregion

        #region Input Methods
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}
