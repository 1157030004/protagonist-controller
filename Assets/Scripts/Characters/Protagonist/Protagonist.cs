using Shadee.ProtagonistController.Data;
using Shadee.ProtagonistController.Utilities;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [RequireComponent(typeof(ProtagonistInput))]
    public class Protagonist : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public ProtagonistSO Data { get; private set; }

        [field: Header("Collisions")]
        [field: SerializeField] public CapsuleColliderUtility ColliderUtility { get; private set; }
        [field: SerializeField] public ProtagonistLayerData LayerData { get; private set; }

        public Rigidbody Rigidbody { get; private set; }
        public ProtagonistInput Input { get; private set; }
        public Transform MainCameraTransform { get; private set; }

        private ProtagonistMovementStateMachine _movementStateMachine;

        private void Awake() 
        {
            Rigidbody = GetComponent<Rigidbody>();
            Input = GetComponent<ProtagonistInput>();

            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CalculateCapsuleColliderDimentions();

            MainCameraTransform = Camera.main.transform;
            _movementStateMachine = new ProtagonistMovementStateMachine(this);    
        }

        private void OnValidate() 
        {
            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CalculateCapsuleColliderDimentions();
        }

        private void Start() 
        {
            _movementStateMachine.ChangeState(_movementStateMachine.IdlingState);
        }

        private void OnTriggerEnter(Collider collider) 
        {
            _movementStateMachine.OnTriggerEnter(collider);    
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
