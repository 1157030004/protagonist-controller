using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [RequireComponent(typeof(ProtagonistInput))]
    public class Protagonist : MonoBehaviour
    {
        public Rigidbody Rigidbody { get; private set; }
        public ProtagonistInput Input { get; private set; }

        private ProtagonistMovementStateMachine _movementStateMachine;

        private void Awake() 
        {
            Rigidbody = GetComponent<Rigidbody>();
            Input = GetComponent<ProtagonistInput>();
            _movementStateMachine = new ProtagonistMovementStateMachine(this);    
        }

        private void Start() 
        {
            _movementStateMachine.ChangeState(_movementStateMachine.IdlingState);
        }

        private void Update() 
        {
            _movementStateMachine.HandleInput();
            _movementStateMachine.Update();
        }

        private void FixedUpdate() 
        {
            _movementStateMachine.PhysicsUpdate();
        }

    }
}
