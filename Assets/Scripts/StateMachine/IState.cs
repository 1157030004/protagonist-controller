using UnityEngine;

namespace Shadee.ProtagonistController.StateMachines
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void HandleInput();
        public void Update();
        public void PhysicsUpdate();
        public void OnAnimationEnterEvent();
        public void OnAnimationExitEvent();
        public void OnAnimationTransitionEvent();
        public void OnTriggerEnter(Collider collider);
    }
}
