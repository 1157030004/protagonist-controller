using UnityEngine;

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
    }
}
