using UnityEngine;

namespace Shadee.ProtagonistController.Characters.Protagonist
{
    [CreateAssetMenu(fileName = "Protagonist", menuName = "Custom/Characters/Protagonist")]
    public class ProtagonistSO : ScriptableObject
    {
        [field: SerializeField] public ProtagonistGroundedData GroundedData { get; private set; }
        [field: SerializeField] public ProtagonistAirboneData AirboneData { get; private set; }
    }
}
