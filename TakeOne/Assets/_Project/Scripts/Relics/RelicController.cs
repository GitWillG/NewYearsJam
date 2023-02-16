using System.Collections.Generic;
using DiceGame.Dice;
using DiceGame.Managers;
using DiceGame.ScriptableObjects;
using DiceGame.ScriptableObjects.Dice;
using DiceGame.Utility;
using UnityEngine;

namespace DiceGame.Relics
{
    public class RelicController : MonoBehaviour, ICollectionElement<RelicController>, IRelic
    {
        [SerializeField] private RelicControllerCollection relicControllerCollection;
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

        private void Awake()
        {
            _encounterEventListener = GetComponent<IEncounterEventListener>();
            _combatEventListener = GetComponent<ICombatEventListener>();
            _diceEventListener = GetComponent<IDiceEventListener>();
            _partyEventListener = GetComponent<IPartyEventListener>();
            
            ((ICollectionElement<RelicController>)this).Register();
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
            
            //TODO: Relic conclusion.
            //Relic available, set a flag so it can activate when needed.
            //Allows for calling "Trigger" functionality when applicable.
            //Some relics will want to activate on button click, others might want to do so on certain events.
        }
        
        private void OnDestroy()
        {
            ((ICollectionElement<RelicController>)this).UnRegister();
        }

        public void OnEncounterStart()
        {
            if(_encounterEventListener == null) return;
            
            _encounterEventListener.OnEncounterStart();
        }

        public void OnEncounterEnd()
        {
            if(_encounterEventListener == null) return;

            _encounterEventListener.OnEncounterEnd();
        }

        public void OnPartyTurnStart(PartyManager partyManager)
        {
            if(_combatEventListener == null) return;
            Debug.Log("CombatEventListener  was found on Relic", this);

            _combatEventListener.OnPartyTurnStart(partyManager);
        }

        public void OnPartyTurnEnd(PartyManager partyManager)
        {
            if(_combatEventListener == null) return;

            _combatEventListener.OnPartyTurnEnd(partyManager);
        }

        public void OnEnemyTurnStart(MonsterManager monsterManager)
        {
            if(_combatEventListener == null) return;

            _combatEventListener.OnEnemyTurnStart(monsterManager);
        }
        
        public void OnEnemyTurnEnd(MonsterManager monsterManager)
        {
            if(_combatEventListener == null) return;

            _combatEventListener.OnEnemyTurnEnd(monsterManager);
        }

        public void OnDealDamage(IDamageable target, IDamageDealer owner)
        {
            if(_combatEventListener == null) return;

            _combatEventListener.OnDealDamage(target, owner);
        }
        
        public void OnBlock(IDamageable target, IDamageDealer owner)
        {
            if(_combatEventListener == null) return;

            _combatEventListener.OnBlock(target, owner);
        }
        
        public void OnDiceRolled(IDiceOwner diceOwner, List<DiceController> diceControllers)
        {
            if(_diceEventListener == null) return;

            _diceEventListener.OnDiceRolled(diceOwner, diceControllers);
        }

        public void OnDiceSelected(DiceController diceController)
        {
            if(_diceEventListener == null) return;

            _diceEventListener.OnDiceSelected(diceController);
        }

        public void OnDiceAttachToSlot(DiceController diceController, DiceSlotHolder diceSlotHolder)
        {
            if(_diceEventListener == null) return;

            _diceEventListener.OnDiceAttachToSlot(diceController, diceSlotHolder);
        }

        public void OnConfirmAllDie(List<DiceController> diceControllers)
        {
            if(_diceEventListener == null) return;

            _diceEventListener.OnConfirmAllDie(diceControllers);
        }

        public void OnPartyCreated(PartyManager partyManager)
        {
            if(_partyEventListener == null) return;

            _partyEventListener.OnPartyCreated(partyManager);
        }

        public void OnPartyDeath(PartyManager partyManager)
        {
            if(_partyEventListener == null) return;

            _partyEventListener.OnPartyDeath(partyManager);
        }
    }
}