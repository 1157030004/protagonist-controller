using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistMovingState : ProtagonistGroundedState
    {
        public ProtagonistMovingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
        }

        #region IState Methods
        override public void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Protagonist.AnimationData.MovingParameterHash);
        }

        override public void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Protagonist.AnimationData.MovingParameterHash);
        } 
        #endregion
    }
}
