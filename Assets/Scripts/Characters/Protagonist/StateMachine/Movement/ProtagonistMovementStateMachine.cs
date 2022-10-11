using Shadee.ProtagonistController.StateMachines;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistMovementStateMachine : StateMachine
    {
        public Protagonist Protagonist { get; }
        public ProtagonistIdlingState IdlingState { get; }
        public ProtagonistWalkingState WalkingState { get; }
        public ProtagonistRunningState RunningState { get; }
        public ProtagonistSprintingState SprintingState { get; }
        
        public ProtagonistMovementStateMachine(Protagonist protagonist)
        {
            Protagonist = protagonist;
            IdlingState = new ProtagonistIdlingState(this);
            WalkingState = new ProtagonistWalkingState(this);
            RunningState = new ProtagonistRunningState(this); 
            SprintingState = new ProtagonistSprintingState(this);
        }

    }
}
