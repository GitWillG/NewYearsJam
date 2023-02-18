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
        private GameEventPropagator _gameEventPropagator;
        
        [ContextMenu("Roll Relic Die")]
        public void RollCombatStartDie()
        {
            diceSlot.GetDiceResults();
            _currentTurnDice = _diceRoller.RollDie(_partyManager, diceSo);
            _gameEventPropagator.OnDiceRolled(_partyManager,new() { _currentTurnDice });

            _currentTurnDice.ONDiceRollResult.AddListener(DieResultFound);
            _currentTurnDice.IsInTray = true;
        }

        private void DieResultFound(int result)
        {
            _currentTurnDice.ONDiceRollResult.RemoveListener(DieResultFound);
            diceSlot.AddDiceToSlot(_currentTurnDice);
            
            _gameEventPropagator.OnRelicDieResult(diceSlot);
        }
    }
}