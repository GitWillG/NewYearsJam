using System;
using System.Collections.Generic;
using System.Linq;
using DiceGame.Dice;
using DiceGame.Relics;
using DiceGame.ScriptableObjects.Dice;
using UnityEngine;

namespace DiceGame.Managers
{
    public class RelicManager : MonoBehaviour
    {
        [SerializeField] private DiceSO diceSo;
        [SerializeField] private DiceSlotHolder diceSlot;
        [SerializeField] private RelicControllerCollection relicControllerCollection;
        
        private DiceController _currentTurnDice;
        private DiceRoller _diceRoller;
        private PartyManager _partyManager;
        private MonsterManager _monsterManager;
        private DiceSelector _diceSelector;
        private DiceDragger _diceDragger;

        private List<RelicController> ListOfPartyRelics => relicControllerCollection.CollectionHashset.ToList();

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
            _currentTurnDice.IsInTray = true;
        }

        private void DieResultFound(int result)
        {
            _currentTurnDice.ONDiceRollResult.RemoveListener(DieResultFound);
            diceSlot.AddDiceToSlot(_currentTurnDice);
        }

        public int GetTurnDieResult()
        {
            return diceSlot.PeekDiceResults().Sum();
        }
    }
}