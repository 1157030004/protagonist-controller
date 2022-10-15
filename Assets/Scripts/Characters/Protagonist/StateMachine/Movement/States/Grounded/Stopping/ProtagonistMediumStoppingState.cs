
namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistMediumStoppingState : ProtagonistStoppingState
    {
        public ProtagonistMediumStoppingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Protagonist.AnimationData.MediumStopParameterHash);

            stateMachine.ReusableData.MovementDecelerationForce = movementData.StopData.MediumDecelerationForce;
            stateMachine.ReusableData.CurrentJumpForce = airboneData.JumpData.MediumForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Protagonist.AnimationData.MediumStopParameterHash);
        }
        #endregion
    }
}
