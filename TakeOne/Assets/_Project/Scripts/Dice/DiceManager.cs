using DiceGame.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace DiceGame.Dice
{
    public class DiceManager : MonoBehaviour
    {
        [FormerlySerializedAs("characterStats")] [SerializeField] private HeroSO characterSoStats;
        [SerializeField] private DiceRoller diceRoller;
        public GameObject selectedDie;
        public int SelectedVal { get; private set; }


        [ContextMenu("Test Roll Dice")]
        public void RollDice()
        {
            for (int i = 0; i < characterSoStats.NumOfDice; i++)
            {
                diceRoller.RollDie(characterSoStats.DiePrefab);
            }
        }
    }
}
