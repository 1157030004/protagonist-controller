using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    public class ProtagonistInput : MonoBehaviour
    {
        public GameInput GameInput { get; private set; }
        public GameInput.ProtagonistActions ProtagonistActions { get; private set; }

        private void Awake() 
        {
            GameInput = new GameInput();
            ProtagonistActions = GameInput.Protagonist; 
        }

        private void OnEnable() 
        {
            GameInput.Enable();
        }

        private void OnDisable() 
        {
            GameInput.Disable();
        }

        public void DisableActionFor(InputAction action, float seconds)
        {
            StartCoroutine(DisableAction(action, seconds));
        }

        private IEnumerator DisableAction(InputAction action, float seconds)
        {
            action.Disable();

            yield return new WaitForSeconds(seconds);

            action.Enable();
        }
    }
}
