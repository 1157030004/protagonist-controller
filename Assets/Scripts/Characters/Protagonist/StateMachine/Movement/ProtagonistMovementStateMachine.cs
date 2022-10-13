using Shadee.ProtagonistController.StateMachines;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistMovementStateMachine : StateMachine
    {
        public Protagonist Protagonist { get; }
        public ProtagonistStateReusableData ReusableData { get; }
        public ProtagonistIdlingState IdlingState { get; }
        public ProtagonistWalkingState WalkingState { get; }
        public ProtagonistRunningState RunningState { get; }
        public ProtagonistSprintingState SprintingState { get; }
        public ProtagonistDashingState DashingState { get; }
        public ProtagonistLightStoppingState LightStoppingState { get; }
        public ProtagonistMediumStoppingState MediumStoppingState { get; }
        public ProtagonistHardStoppingState HardStoppingState { get; }

        public ProtagonistLightLandingState LightLandingState { get; }
        public ProtagonistRollingState RollingState { get; }
        public ProtagonistHardLandingState HardLandingState { get; }

        public ProtagonistJumpingState JumpingState { get; }
        public ProtagonistFallingState FallingState { get; }
        
        public ProtagonistMovementStateMachine(Protagonist protagonist)
        {
            Protagonist = protagonist;
            ReusableData = new ProtagonistStateReusableData();

            IdlingState = new ProtagonistIdlingState(this);
            DashingState = new ProtagonistDashingState(this);

            WalkingState = new ProtagonistWalkingState(this);
            RunningState = new ProtagonistRunningState(this); 
            SprintingState = new ProtagonistSprintingState(this);

            LightStoppingState = new ProtagonistLightStoppingState(this);
            MediumStoppingState = new ProtagonistMediumStoppingState(this);
            HardStoppingState = new ProtagonistHardStoppingState(this);

            LightLandingState = new ProtagonistLightLandingState(this);
            RollingState = new ProtagonistRollingState(this);
            HardLandingState = new ProtagonistHardLandingState(this);
            
            JumpingState = new ProtagonistJumpingState(this);
            FallingState = new ProtagonistFallingState(this);
        }

    }
}
