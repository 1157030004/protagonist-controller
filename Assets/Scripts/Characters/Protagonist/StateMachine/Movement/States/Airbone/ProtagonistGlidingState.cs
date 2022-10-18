using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistGlidingState : ProtagonistAirboneState
    {
        private ProtagonistGlideData glideData;
        public ProtagonistGlidingState(ProtagonistMovementStateMachine protagonistMovementStateMachine) : base(protagonistMovementStateMachine)
        {
            glideData = airboneData.GlideData;
        }

        #region IState Methods

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Protagonist.AnimationData.GlideParameterHash);
            stateMachine.Protagonist.HeldItemData.glider.SetActive(true);

            stateMachine.ReusableData.MovementSpeedModifier = 0f;

        }
        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Protagonist.AnimationData.GlideParameterHash);
            stateMachine.Protagonist.HeldItemData.glider.SetActive(false);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            RotateTowardsTargetRotation();

            Glide();
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();
            stateMachine.Protagonist.Input.ProtagonistActions.Jump.started += OnGlidingStopped;
        }

        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            stateMachine.Protagonist.Input.ProtagonistActions.Jump.started -= OnGlidingStopped;
        }

        #endregion

        #region Main Methods
        private void Glide()
        {
            Vector3 glideForce = glideData.GlideForce;
            Vector3 glideDirection = stateMachine.Protagonist.transform.forward;
            
            UpdateTargetRotation(new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, 0f));
            glideDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);

            glideForce.x *= glideDirection.x;
            glideForce.z *= glideDirection.z;

            ResetVelocity();

            Vector3 verticalVelocity = stateMachine.Protagonist.Rigidbody.velocity - Vector3.ProjectOnPlane(stateMachine.Protagonist.Rigidbody.velocity, stateMachine.Protagonist.transform.up);
            stateMachine.Protagonist.Rigidbody.AddForce(verticalVelocity, ForceMode.VelocityChange);
            
            Vector3 forwardDrag = stateMachine.Protagonist.Rigidbody.velocity - Vector3.ProjectOnPlane(stateMachine.Protagonist.Rigidbody.velocity, stateMachine.Protagonist.transform.forward);
            stateMachine.Protagonist.Rigidbody.AddForce(glideForce, ForceMode.VelocityChange);


            // Vector3 sideDrag = stateMachine.Protagonist.Rigidbody.velocity - Vector3.ProjectOnPlane(stateMachine.Protagonist.Rigidbody.velocity, stateMachine.Protagonist.transform.right);
            // stateMachine.Protagonist.Rigidbody.AddForce(-sideDrag * Time.deltaTime, ForceMode.VelocityChange);

            // stateMachine.Protagonist.Rigidbody.AddForce(limitedVelocity, ForceMode.VelocityChange);


            // Vector2 horizontalvelocity = GetProtagonistHorizontalVelocity();
            // Vector2 offsetVelocity =  (stateMachine.ReusableData.MovementInput * 1f) - horizontalvelocity;
            // stateMachine.Protagonist.Rigidbody.AddForce(new Vector3(offsetVelocity.x, 0, offsetVelocity.y), ForceMode.VelocityChange);

            // if(stateMachine.Protagonist.Rigidbody.velocity.y < -1f)
            // {
            //     stateMachine.Protagonist.Rigidbody.AddForce(new Vector3(0, 70, 0));
            // }


        }
        #endregion

        #region Input Methods
        protected override void OnMovementPerformed(InputAction.CallbackContext context)
        {
            base.OnMovementPerformed(context);
        }
        private void OnGlidingStopped(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }
        #endregion
    }
}
