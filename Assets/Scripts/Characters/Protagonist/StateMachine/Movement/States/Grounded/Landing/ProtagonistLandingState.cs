
namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistLandingState : ProtagonistGroundedState
    {
        public ProtagonistLandingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Protagonist.AnimationData.LandingParameterHash);

            DisableCameraRecentering();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Protagonist.AnimationData.LandingParameterHash);
        }
        #endregion
    }
}
