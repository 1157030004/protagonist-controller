using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistMovingState : ProtagonistGroundedState
    {
        public ProtagonistMovingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
        }
    }
}
