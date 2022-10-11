namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistIdlingState : ProtagonistMovementState
    {
        public ProtagonistIdlingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
