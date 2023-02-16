using DiceGame.ScriptableObjects.Conditions;
using UnityEngine;

namespace DiceGame.Relics
{
    [CreateAssetMenu(menuName = "Create RelicSO", fileName = "RelicSO", order = 0)]
    public class RelicSO : ScriptableObject
    {
        [SerializeField] private Texture2D relicTexture;
        [SerializeField] private string relicName;
        [TextArea]
        [SerializeField] private string relicDescription;
        [SerializeField] private Condition activationCondition;

        public Texture2D RelicTexture => relicTexture;

        public string RelicName => relicName;

        public string RelicDescription => relicDescription;

        public Condition ActivationCondition => activationCondition;
    }
}