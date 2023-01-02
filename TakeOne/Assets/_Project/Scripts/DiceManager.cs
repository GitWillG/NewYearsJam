using UnityEngine;

namespace Gameplay
{
    public class DiceManager : MonoBehaviour
    {
        [SerializeField] private PlayableCharacter characterStats;
        [SerializeField] private DiceRoller diceRoller;
        public GameObject selectedDie;
        public int selectedVal { get; private set; }


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
