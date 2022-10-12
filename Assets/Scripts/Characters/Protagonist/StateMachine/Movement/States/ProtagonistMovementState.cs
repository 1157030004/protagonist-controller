
using Shadee.ProtagonistController.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistMovementState : IState
    {
        protected ProtagonistMovementStateMachine stateMachine;

        protected ProtagonistGroundedData movementData;

        public ProtagonistMovementState(ProtagonistMovementStateMachine protagonistMovementStateMachine)
        {
            stateMachine = protagonistMovementStateMachine;
            movementData = stateMachine.Protagonist.Data.GroundedData;

            InitializeData();
        }

        private void InitializeData()
        {
            stateMachine.ReusableData.TimeToReachTargetRotation = movementData.BaseRotationData.TargetRotationReachTime;
        }

        #region IState Methods
        public virtual void Enter()
        {
            Debug.Log("State: " + GetType().Name);

            AddInputActionCallbacks();
        }

        public virtual void Exit()
        {
            RemoveInputActionCallbacks();
        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void Update()
        {

        }

        public virtual void PhysicsUpdate()
        {
            Move();
        }
        
        public virtual void OnAnimationEnterEvent()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }
        #endregion

        #region Main Methods
        private void ReadMovementInput() 
        {
            stateMachine.ReusableData.MovementInput = stateMachine.Protagonist.Input.ProtagonistActions.Movement.ReadValue<Vector2>();
        }

        private void Move()
        {
            if(stateMachine.ReusableData.MovementInput == Vector2.zero || stateMachine.ReusableData.MovementSpeedModifier == 0)
                return;
            
            Vector3 movementDirection = GetMovementInputDirection();

            float targetRorattionYAngle = Rotate(movementDirection);

            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRorattionYAngle);

            float movementSpeed = GetMovementSpeed();

            Vector3 currentProtagonistHorizontalVelocity = GetProtagonistHorizontalVelocity();

            stateMachine.Protagonist.Rigidbody.AddForce(targetRotationDirection * movementSpeed - currentProtagonistHorizontalVelocity, ForceMode.VelocityChange);

        }

        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpgradeTargetRotation(direction);

            RotateTowardsTargetRotation();

            return directionAngle;
        }

        private void UpdateTargetRotationData(float targetAngle)
        {
            stateMachine.ReusableData.CurrentTargetRotation.y = targetAngle;

            stateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
        }

        private static float GetDirectionAngle(Vector3 direction)
        {
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            if (directionAngle < 0f)
                directionAngle += 360f;
            return directionAngle;
        }

        private float AddCameraRotationToAngle(float angle)
        {
            angle += stateMachine.Protagonist.MainCameraTransform.eulerAngles.y;
            if (angle > 360f)
                angle -= 360f;
            return angle;
        }
        #endregion

        #region Reusable Methods
        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, stateMachine.ReusableData.MovementInput.y);
        }

        protected float GetMovementSpeed()
        {
            return movementData.BaseSpeed * stateMachine.ReusableData.MovementSpeedModifier * stateMachine.ReusableData.MovementOnSlopesSpeedModifier;
        }

        protected Vector3 GetProtagonistHorizontalVelocity()
        {
            return new Vector3(stateMachine.Protagonist.Rigidbody.velocity.x, 0f, stateMachine.Protagonist.Rigidbody.velocity.z);
        }

        protected Vector3 GetProtagonistVerticalVelocity()
        {
            return new Vector3(0f, stateMachine.Protagonist.Rigidbody.velocity.y, 0f);
        }

        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle = stateMachine.Protagonist.Rigidbody.rotation.eulerAngles.y;

            if(currentYAngle == stateMachine.ReusableData.CurrentTargetRotation.y)
                return;
            
            float smoothYAngle = Mathf.SmoothDampAngle(
                currentYAngle, 
                stateMachine.ReusableData.CurrentTargetRotation.y, 
                ref stateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y, 
                stateMachine.ReusableData.TimeToReachTargetRotation.y - stateMachine.ReusableData.DampedTargetRotationPassedTime.y);

            stateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0f, smoothYAngle, 0f);

            stateMachine.Protagonist.Rigidbody.MoveRotation(targetRotation);

        }
        
        protected float UpgradeTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            float directionAngle = GetDirectionAngle(direction);

            if(shouldConsiderCameraRotation)
            {
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }

            if (directionAngle != stateMachine.ReusableData.CurrentTargetRotation.y)
            {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }

        protected Vector3 GetTargetRotationDirection(float targetAngle)
        {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        
        protected void ResetVelocity()
        {
            stateMachine.Protagonist.Rigidbody.velocity = Vector3.zero;
        }

        protected virtual void AddInputActionCallbacks()
        {
            stateMachine.Protagonist.Input.ProtagonistActions.WalkToggle.started += OnWalkToggleStarted;
        }

        protected virtual void RemoveInputActionCallbacks()
        {
            stateMachine.Protagonist.Input.ProtagonistActions.WalkToggle.started -= OnWalkToggleStarted;   
        }
        
        protected void DecelerateHorizontally()
        {
            Vector3 protagonistHorizontalVelocity = GetProtagonistHorizontalVelocity();
            stateMachine.Protagonist.Rigidbody.AddForce(-protagonistHorizontalVelocity * stateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 protagonistHorizontalVelocity = GetProtagonistHorizontalVelocity();
            Vector2 protagonistHorizontalMovement = new Vector2(protagonistHorizontalVelocity.x, protagonistHorizontalVelocity.z);
            
            return protagonistHorizontalMovement.magnitude > minimumMagnitude;
        }
        #endregion

        #region Input Methods
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            stateMachine.ReusableData.ShouldWalk = !stateMachine.ReusableData.ShouldWalk;
        }
        #endregion
    }
}
