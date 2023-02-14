using System;
using System.Linq;
using DiceGame.Dice;
using DiceGame.ScriptableObjects.Dice;
using UnityEngine;

namespace DiceGame.Managers
{
    public class RelicManager : MonoBehaviour
    {
        [SerializeField] private DiceSO diceSo;
        [SerializeField] private DiceSlotHolder diceSlot;
        
        private DiceController _currentTurnDice;
        private DiceRoller _diceRoller;
        private PartyManager _partyManager;
        
        //TODO: keep a collection of all the party member relics. Probably through the ScriptableObject.
        //When die result is found, or when the die snaps to the anchor for the relics. 
        //Highlight all the relics that are active for the combat.

        private void Awake()
        {
            _diceRoller = FindObjectOfType<DiceRoller>();
            _partyManager = FindObjectOfType<PartyManager>();
        }

        [ContextMenu("Roll Relic Die")]
        public void RollCombatStartDie()
        {
            diceSlot.GetDiceResults();
            _currentTurnDice = _diceRoller.RollDie(_partyManager, diceSo);
            _currentTurnDice.ONDiceRollResult.AddListener(DieResultFound);
        }

        private void DieResultFound(int result)
        {
            _currentTurnDice.ONDiceRollResult.RemoveListener(DieResultFound);
            //Do stuff here
            diceSlot.AddDiceToSlot(_currentTurnDice);
        }

        public int GetTurnDieResult()
        {
            return diceSlot.PeekDiceResults().Sum();
        }
    }
}