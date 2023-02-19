using System;
using DiceGame.Dice;
using DiceGame.ScriptableObjects;
using DiceGame.ScriptableObjects.Dice;
using DiceGame.Utility;
using UnityEngine;

namespace DiceGame.Managers
{
    public class RelicManager : MonoBehaviour, IAllGameEventListener, ICollectionElement<IAllGameEventListener>
    {
        [SerializeField] private DiceSO diceSo;
        [SerializeField] private DiceSlotHolder diceSlot;
        [SerializeField] private AllGameEventListenerCollection allGameEventListenerCollection;

        private DiceController _currentTurnDice;
        private DiceRoller _diceRoller;
        private PartyManager _partyManager;
        private GameEventPropagator _gameEventPropagator;
        
        public CollectionExposerSO<IAllGameEventListener> CollectionReference => allGameEventListenerCollection;

        private void Awake()
        {
            _diceRoller = FindObjectOfType<DiceRoller>();
            _partyManager = FindObjectOfType<PartyManager>();
            _gameEventPropagator = FindObjectOfType<GameEventPropagator>();
            
            ((ICollectionElement<IAllGameEventListener>)this).Register();

        }

        public void OnEncounterStart()
        {
            RollRelicDie();
        }

        [ContextMenu("Roll Relic Die")]
        public void RollRelicDie()
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
            
            _gameEventPropagator.OnRelicDieResult(diceSlot.PeekDiceResults());
        }

        private void OnDestroy()
        {
            ((ICollectionElement<IAllGameEventListener>)this).UnRegister();
        }
    }
}