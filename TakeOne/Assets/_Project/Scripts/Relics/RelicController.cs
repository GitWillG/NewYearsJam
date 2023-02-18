using System.Collections.Generic;
using DiceGame.Dice;
using DiceGame.Managers;
using DiceGame.ScriptableObjects;
using DiceGame.ScriptableObjects.Dice;
using DiceGame.Utility;
using UnityEngine;

namespace DiceGame.Relics
{
    //TODO: Need to find a way to spawn relics. RelicManager 
    public class RelicController : MonoBehaviour, ICollectionElement<RelicController>
    {
        [SerializeField] private RelicControllerCollection relicControllerCollection;
        [SerializeField] private RelicSO tempRelicData; //TODO: Remove this once we spawn relics normally
        private RelicSO _relicSo;

        public CollectionExposerSO<RelicController> CollectionReference
        {
            get => relicControllerCollection;
            set => relicControllerCollection = (RelicControllerCollection)value;
        }

        private IEncounterEventListener _encounterEventListener;
        private ICombatEventListener _combatEventListener;
        private IDiceEventListener _diceEventListener;
        private IPartyEventListener _partyEventListener;
        
        private IRelic _relic;

        private void Awake()
        {
            _encounterEventListener = GetComponent<IEncounterEventListener>();
            _combatEventListener = GetComponent<ICombatEventListener>();
            _diceEventListener = GetComponent<IDiceEventListener>();
            _partyEventListener = GetComponent<IPartyEventListener>();
            
            _relic = GetComponent<IRelic>();
            
            ((ICollectionElement<RelicController>)this).Register();
            
            if (tempRelicData != null && _relicSo == null)
            {
                Initialize(tempRelicData);
            }
        }
        
        public void Initialize(RelicSO relicSo)
        {
            _relicSo = relicSo;
        }

        public void OnRelicDieResultRolled(List<int> dieResults)
        {
            _relicSo.ActivationCondition.EvaluateConditions(dieResults);
            var passingDie = _relicSo.ActivationCondition.GetPassingDie();
            
            if(passingDie == null || passingDie.Count < 1) return;

            _relic.CanTrigger = true;
        }
        
        private void OnDestroy()
        {
            ((ICollectionElement<RelicController>)this).UnRegister();
        }

        //TODO: Refactoring required. I feel this is too many things for this single class. Maybe we extract all the functionality below in to its own class?
        //We can somehow reuse it for the Relic Manager too somehow? Gotta think more about the specifics before making the decision.
        #region GameEvents
        
        public void OnEncounterStart()
        {
            _encounterEventListener?.OnEncounterStart();
        }

        public void OnEncounterEnd()
        {
            _encounterEventListener?.OnEncounterEnd();
        }

        public void OnPartyTurnStart(PartyManager partyManager)
        {
            _combatEventListener?.OnPartyTurnStart(partyManager);
        }

        public void OnPartyTurnEnd(PartyManager partyManager)
        {
            _combatEventListener?.OnPartyTurnEnd(partyManager);
        }

        public void OnEnemyTurnStart(MonsterManager monsterManager)
        {
            _combatEventListener?.OnEnemyTurnStart(monsterManager);
        }
        
        public void OnEnemyTurnEnd(MonsterManager monsterManager)
        {
            _combatEventListener?.OnEnemyTurnEnd(monsterManager);
        }

        public void OnDealDamage(IDamageable target, IDamageDealer owner)
        {
            _combatEventListener?.OnDealDamage(target, owner);
        }
        
        public void OnBlock(IDamageable target, IDamageDealer owner)
        {
            _combatEventListener?.OnBlock(target, owner);
        }
        
        public void OnDiceRolled(IDiceOwner diceOwner, List<DiceController> diceControllers)
        {
            if(_diceEventListener == null) return;
            
            _diceEventListener.OnDiceRolled(diceOwner, diceControllers);
        }

        public void OnDiceSelected(DiceController diceController)
        {
            _diceEventListener?.OnDiceSelected(diceController);
        }

        public void OnDiceAttachToSlot(DiceController diceController, DiceSlotHolder diceSlotHolder)
        {
            _diceEventListener?.OnDiceAttachToSlot(diceController, diceSlotHolder);
        }

        public void OnConfirmAllDie(List<DiceController> diceControllers)
        {
            _diceEventListener?.OnConfirmAllDie(diceControllers);
        }

        public void OnPartyCreated(PartyManager partyManager)
        {
            _partyEventListener?.OnPartyCreated(partyManager);
        }

        public void OnPartyDeath(PartyManager partyManager)
        {
            _partyEventListener?.OnPartyDeath(partyManager);
        }
        #endregion

    }
}