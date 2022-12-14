
using System;
using System.Collections.Generic;
using Shadee.ProtagonistController.Data;
using Shadee.ProtagonistController.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistMovementState : IState
    {
        protected ProtagonistMovementStateMachine stateMachine;

        protected ProtagonistGroundedData movementData;
        protected ProtagonistAirboneData airboneData;

        public ProtagonistMovementState(ProtagonistMovementStateMachine protagonistMovementStateMachine)
        {
            stateMachine = protagonistMovementStateMachine;
            movementData = stateMachine.Protagonist.Data.GroundedData;
            airboneData = stateMachine.Protagonist.Data.AirboneData;

            SetBaseCameraRecenteringData();

            InitializeData();
        }

        private void InitializeData()
        {
            SetBaseRotationData();
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
        
        public virtual void OnTriggerEnter(Collider collider)
        {
            if(stateMachine.Protagonist.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGround(collider);

                return;
            }
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            if(stateMachine.Protagonist.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGroundExited(collider);

                return;
            }
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

            float targetRotationYAngle = Rotate(movementDirection);

            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

            float movementSpeed = GetMovementSpeed();

            Vector3 currentProtagonistHorizontalVelocity = GetProtagonistHorizontalVelocity();

            stateMachine.Protagonist.Rigidbody.AddForce(targetRotationDirection * movementSpeed - currentProtagonistHorizontalVelocity, ForceMode.VelocityChange);

        }

        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);

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

        private static float GetGlidingDirectionAngle(Vector3 direction)
        {
            direction.z = 0;
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

        protected void StartAnimation(int animationHash)
        {
            stateMachine.Protagonist.Animator.SetBool(animationHash, true);
        }
        protected void StopAnimation(int animationHash)
        {
            stateMachine.Protagonist.Animator.SetBool(animationHash, false);
        }
        protected void SetBaseCameraRecenteringData()
        {
            stateMachine.ReusableData.BackwardsCameraRecenteringData = movementData.BackwardsCameraRecenteringData;
            stateMachine.ReusableData.SidewaysCameraRecenteringData = movementData.SidewaysCameraRecenteringData;
        }

        protected void SetBaseRotationData()
        {
            stateMachine.ReusableData.RotationData = movementData.BaseRotationData;

            stateMachine.ReusableData.TimeToReachTargetRotation = stateMachine.ReusableData.RotationData.TargetRotationReachTime;
        }
        
        protected virtual void AddInputActionCallbacks()
        {
            stateMachine.Protagonist.Input.ProtagonistActions.WalkToggle.started += OnWalkToggleStarted;
            stateMachine.Protagonist.Input.ProtagonistActions.Look.started += OnMouseMovementStarted;
            stateMachine.Protagonist.Input.ProtagonistActions.Movement.performed += OnMovementPerformed;
            stateMachine.Protagonist.Input.ProtagonistActions.Movement.canceled += OnMovementCanceled;
        }

        protected virtual void RemoveInputActionCallbacks()
        {
            stateMachine.Protagonist.Input.ProtagonistActions.WalkToggle.started -= OnWalkToggleStarted;
            stateMachine.Protagonist.Input.ProtagonistActions.Look.started -= OnMouseMovementStarted;
            stateMachine.Protagonist.Input.ProtagonistActions.Movement.performed -= OnMovementPerformed;
            stateMachine.Protagonist.Input.ProtagonistActions.Movement.canceled -= OnMovementCanceled;   
        }
        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, stateMachine.ReusableData.MovementInput.y);
        }

        protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
        {
            float movementSpeed = movementData.BaseSpeed * stateMachine.ReusableData.MovementSpeedModifier;

            if(shouldConsiderSlopes)
            {
                movementSpeed *= stateMachine.ReusableData.MovementOnSlopesSpeedModifier;
            }
            return movementSpeed;
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
        
        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
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
        
        protected void ResetVerticalVelocity()
        {
            Vector3 protagonistHorizontalVelocity = GetProtagonistHorizontalVelocity();

            stateMachine.Protagonist.Rigidbody.velocity = protagonistHorizontalVelocity;
        }

        protected void DecelerateHorizontally()
        {
            Vector3 protagonistHorizontalVelocity = GetProtagonistHorizontalVelocity();
            stateMachine.Protagonist.Rigidbody.AddForce(-protagonistHorizontalVelocity * stateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }
        protected void DecelerateVertically()
        {
            Vector3 protagonistVerticalVelocity = GetProtagonistVerticalVelocity();
            stateMachine.Protagonist.Rigidbody.AddForce(-protagonistVerticalVelocity * stateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 protagonistHorizontalVelocity = GetProtagonistHorizontalVelocity();
            Vector2 protagonistHorizontalMovement = new Vector2(protagonistHorizontalVelocity.x, protagonistHorizontalVelocity.z);
            
            return protagonistHorizontalMovement.magnitude > minimumMagnitude;
        }

        protected bool IsMovingUp(float minimumVelocity = 0.1f)
        {
            return GetProtagonistVerticalVelocity().y > minimumVelocity;
        }
        protected bool IsMovingDown(float minimumVelocity = 0.1f)
        {
            return GetProtagonistVerticalVelocity().y < -minimumVelocity;
        }

        protected virtual void OnContactWithGround(Collider collider)
        {
        }
        
        protected virtual void OnContactWithGroundExited(Collider collider)
        {
        }
        
        protected void UpdateCameraRecenteringState(Vector2 movementInput)
        {
            if(movementInput == Vector2.zero)
                return;

            if(movementInput == Vector2.up)
            {
                DisableCameraRecentering();

                return;
            }

            float cameraVerticalAngle = stateMachine.Protagonist.MainCameraTransform.eulerAngles.x;
            if(cameraVerticalAngle >= 270f)
            {
                cameraVerticalAngle -= 360f;
            } 
            cameraVerticalAngle = Mathf.Abs(cameraVerticalAngle);

            if(movementInput == Vector2.down)
            {
                SetCameraRecenteringState(cameraVerticalAngle, stateMachine.ReusableData.BackwardsCameraRecenteringData);
                return;
            }

            SetCameraRecenteringState(cameraVerticalAngle, stateMachine.ReusableData.SidewaysCameraRecenteringData);
        }

        protected void EnableCameraRecentering(float waitTime = -1f, float recenteringTime = -1f)
        {
            float movementSpeed = GetMovementSpeed();

            if(movementSpeed == 0f)
            {
                movementSpeed = movementData.BaseSpeed;
            }
            stateMachine.Protagonist.CameraUtility.EnableRecentering(waitTime, recenteringTime, movementData.BaseSpeed, movementSpeed);
        }

        protected void DisableCameraRecentering()
        {
            stateMachine.Protagonist.CameraUtility.DisableRecentering();
        }

        protected void SetCameraRecenteringState(float cameraVerticalAngle, List<ProtagonistCameraRecenteringData> cameraRecenteringData)
        {
            foreach (ProtagonistCameraRecenteringData recenteringData in cameraRecenteringData)
            {
                if (!recenteringData.IsWithinRange(cameraVerticalAngle))
                    continue;

                EnableCameraRecentering(recenteringData.WaitTime, recenteringData.RecenteringTime);
                return;
            }

            DisableCameraRecentering();
        }
        #endregion

        #region Input Methods
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            stateMachine.ReusableData.ShouldWalk = !stateMachine.ReusableData.ShouldWalk;
        }

        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            DisableCameraRecentering();
        }
        
        private void OnMouseMovementStarted(InputAction.CallbackContext context)
        {
            UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);
        }

        protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
        {
            UpdateCameraRecenteringState(context.ReadValue<Vector2>());
        }
        
        #endregion
    }
}
