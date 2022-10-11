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
        public ProtagonistMovementState(ProtagonistMovementStateMachine protagonistMovementStateMachine)
        {
            stateMachine = protagonistMovementStateMachine;
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

            float movementSpeed = GetMovementSpeed();

            Vector3 currentProtagonistHorizontalVelocity = GetProtagonistHorizontalVelocity();

            stateMachine.Protagonist.Rigidbody.AddForce(movementDirection * movementSpeed - currentProtagonistHorizontalVelocity, ForceMode.VelocityChange);

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
        #endregion
    }
}
