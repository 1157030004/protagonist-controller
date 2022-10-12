
namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistLightStoppingState : ProtagonistStoppingState
    {
        public ProtagonistLightStoppingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementDecelerationForce = movementData.StopData.LightDecelerationForce;
        }
        #endregion
    }
}
