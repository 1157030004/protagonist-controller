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
        
        public ProtagonistMovementStateMachine(Protagonist protagonist)
        {
            Protagonist = protagonist;
            ReusableData = new ProtagonistStateReusableData();
            IdlingState = new ProtagonistIdlingState(this);
            WalkingState = new ProtagonistWalkingState(this);
            RunningState = new ProtagonistRunningState(this); 
            SprintingState = new ProtagonistSprintingState(this);
            DashingState = new ProtagonistDashingState(this);
        }

    }
}
