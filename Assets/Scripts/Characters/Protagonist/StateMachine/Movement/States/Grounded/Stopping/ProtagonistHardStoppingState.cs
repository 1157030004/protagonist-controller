
namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistHardStoppingState : ProtagonistStoppingState
    {
        public ProtagonistHardStoppingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Protagonist.AnimationData.HardStopParameterHash);

            stateMachine.ReusableData.MovementDecelerationForce = movementData.StopData.HardDecelerationForce;
            stateMachine.ReusableData.CurrentJumpForce = airboneData.JumpData.StrongForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Protagonist.AnimationData.HardStopParameterHash);
        }
        #endregion

        #region Reusable Methods
        protected override void OnMove()
        {
            if(stateMachine.ReusableData.ShouldWalk)
                return;

            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion
    }
}
