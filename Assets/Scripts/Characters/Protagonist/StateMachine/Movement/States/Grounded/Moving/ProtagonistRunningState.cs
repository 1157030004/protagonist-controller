using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistRunningState : ProtagonistMovingState
    {
        private ProtagonistSprintData sprintData;
        private float startTime;
        public ProtagonistRunningState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
            sprintData = movementData.SprintData;
        }

        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = movementData.RunData.SpeedModifier;
            base.Enter();

            StartAnimation(stateMachine.Protagonist.AnimationData.RunParameterHash);

            stateMachine.ReusableData.CurrentJumpForce = airboneData.JumpData.MediumForce;
            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Protagonist.AnimationData.RunParameterHash);
        }

        public override void Update()
        {
            base.Update();

            if(!stateMachine.ReusableData.ShouldWalk)
                return;

            if(Time.time < startTime + sprintData.RunToWalkTime)
                return;

            StopRunning();
        }
        #endregion

        #region Main Methods
        private void StopRunning()
        {
            if(stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.IdlingState); // TODO: Change to stopping state
                return;
            }

            stateMachine.ChangeState(stateMachine.WalkingState);
        }
        #endregion

        #region Input Methods

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.MediumStoppingState);
            base.OnMovementCanceled(context);
        }
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            stateMachine.ChangeState(stateMachine.WalkingState);
        }
        #endregion

    }
}
