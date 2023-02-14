using System;
using System.Collections.Generic;
using DiceGame.Dice;
using DiceGame.Managers;
using DiceGame.ScriptableObjects;
using DiceGame.ScriptableObjects.Dice;
using DiceGame.Utility;
using UnityEngine;

namespace DiceGame.Relics
{
    public class RelicController : MonoBehaviour, IEncounterEventListener, ICombatEventListener, IDiceEventListener, IPartyEventListener, ICollectionElement<RelicController>
    {
        //Sets up all the bindings for a relic.
        //Scripts for certain behaviours.
        [SerializeField] private RelicControllerCollection relicControllerCollection;

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

        private void OnDestroy()
        {
            ((ICollectionElement<RelicController>)this).UnRegister();
        }

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

        public void OnDealDamage(IDamageable target, IDamageDealer owner, out int damage)
        {
            damage = 0;
            _combatEventListener?.OnDealDamage(target, owner, out damage);
        }
        
        public void OnBlock(IDamageable target, IDamageDealer owner, out int damageBlocked)
        {
            damageBlocked = 0;
            _combatEventListener?.OnBlock(target, owner, out damageBlocked);
        }
        
        public void OnDiceRolled(IDiceOwner diceOwner, List<DiceController> diceControllers)
        {
            _diceEventListener?.OnDiceRolled(diceOwner, diceControllers);
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

        public void OnPartyDie(PartyManager partyManager)
        {
            _partyEventListener?.OnPartyDie(partyManager);
        }

    }
}