
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

            stateMachine.ReusableData.MovementDecelerationForce = movementData.StopData.MediumDecelerationForce;
        }
        #endregion
    }
}
