using System.Collections;
using System.Collections.Generic;
using Shadee.ProtagonistController.Characters.Protagonist;
using UnityEngine;

namespace Shadee.ProtagonistController.Utilities
{
    public class ProtagonistAnimationEventTrigger : MonoBehaviour
    {
        private Protagonist protagonist;

        private void Awake() 
        {
            protagonist = transform.parent.GetComponent<Protagonist>();    
        }

        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            if(IsInAnimationTransition())
                return;
            protagonist.OnMovementStateAnimationEnterEvent();
        }

        public void TriggerOnMovementStateAnimationExitEvent()
        {
            if(IsInAnimationTransition())
                return;
            protagonist.OnMovementStateAnimationExitEvent();
        }
        
        public void TriggerOnMovementStateAnimationTransitionEvent()
        {
            if(IsInAnimationTransition())
                return;
            protagonist.OnMovementStateAnimationTransitionEvent();
        }

        private bool IsInAnimationTransition(int layerIndex = 0)
        {
            return protagonist.Animator.IsInTransition(layerIndex);
        }
    }
}
