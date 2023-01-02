using UnityEngine;

namespace Gameplay
{
    public class DiceManager : MonoBehaviour
    {
        [SerializeField] private PlayableCharacter characterStats;
        [SerializeField] private DiceRoller diceRoller;

        [ContextMenu("Test Roll Dice")]
        public void RollDice()
        {
            for (int i = 0; i < characterStats.NumOfDice; i++)
            {
                diceRoller.RollDie(characterStats.DiePrefab);
            }
        }
    }
}
