using System;
using Shadee.ProtagonistController.StateMachines;
using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistMovementState : IState
    {
        protected ProtagonistMovementStateMachine stateMachine;
        protected Vector2 movementInput;
        protected float baseSpeed = 5f;
        protected float speedModifier = 1f;
        protected Vector3 currentTargetRotation;
        protected Vector3 timeToReachTargetRotation;
        protected Vector3 dampedTargetRotationCurrentVelocity;
        protected Vector3 dampedTargetRotationPassedTime;
        public ProtagonistMovementState(ProtagonistMovementStateMachine protagonistMovementStateMachine)
        {
            stateMachine = protagonistMovementStateMachine;

            InitializeData();
        }

        private void InitializeData()
        {
            timeToReachTargetRotation.y = 0.14f;
        }

        #region IState Methods
        public virtual void Enter()
        {
            Debug.Log("State: " + GetType().Name);
        }

        public virtual void Exit()
        {

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
        #endregion

        #region Main Methods
        private void ReadMovementInput() 
        {
            movementInput = stateMachine.Protagonist.Input.ProtagonistActions.Movement.ReadValue<Vector2>();
        }

        private void Move()
        {
            if(movementInput == Vector2.zero || speedModifier == 0)
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
            currentTargetRotation.y = targetAngle;

            dampedTargetRotationPassedTime.y = 0f;
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
            return new Vector3(movementInput.x, 0f, movementInput.y);
        }

        protected float GetMovementSpeed()
        {
            return baseSpeed * speedModifier;
        }

        protected Vector3 GetProtagonistHorizontalVelocity()
        {
            return new Vector3(stateMachine.Protagonist.Rigidbody.velocity.x, 0f, stateMachine.Protagonist.Rigidbody.velocity.z);
        }

        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle = stateMachine.Protagonist.Rigidbody.rotation.eulerAngles.y;

            if(currentYAngle == currentTargetRotation.y)
                return;
            
            float smoothYAngle = Mathf.SmoothDampAngle(
                currentYAngle, 
                currentTargetRotation.y, 
                ref dampedTargetRotationCurrentVelocity.y, 
                timeToReachTargetRotation.y - dampedTargetRotationPassedTime.y);

            dampedTargetRotationPassedTime.y += Time.deltaTime;

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

            if (directionAngle != currentTargetRotation.y)
            {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }

        protected Vector3 GetTargetRotationDirection(float targetAngle)
        {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        #endregion
    }
}
