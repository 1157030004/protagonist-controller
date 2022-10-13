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
        [field: SerializeField] public ProtagonistCapsuleColliderUtility ColliderUtility { get; private set; }
        [field: SerializeField] public ProtagonistLayerData LayerData { get; private set; }

        public Rigidbody Rigidbody { get; private set; }
        public ProtagonistInput Input { get; private set; }
        public Transform MainCameraTransform { get; private set; }

        private ProtagonistMovementStateMachine movementStateMachine;

        private void Awake() 
        {
            Rigidbody = GetComponent<Rigidbody>();
            Input = GetComponent<ProtagonistInput>();

            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CalculateCapsuleColliderDimentions();

            MainCameraTransform = Camera.main.transform;
            movementStateMachine = new ProtagonistMovementStateMachine(this);    
        }

        private void OnValidate() 
        {
            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CalculateCapsuleColliderDimentions();
        }

        private void Start() 
        {
            movementStateMachine.ChangeState(movementStateMachine.IdlingState);
        }

        private void OnTriggerEnter(Collider collider) 
        {
            movementStateMachine.OnTriggerEnter(collider);    
        }

        private void OnTriggerExit(Collider collider) 
        {
            movementStateMachine.OnTriggerExit(collider);    
        }

        private void Update() 
        {
            movementStateMachine.HandleInput();
            movementStateMachine.Update();
        }

        private void FixedUpdate() 
        {
            movementStateMachine.PhysicsUpdate();
        }

    }
}
