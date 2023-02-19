using System.Collections.Generic;
using System.Linq;
using DiceGame.Dice;
using DiceGame.ScriptableObjects.Dice;
using DiceGame.Utility;
using UnityEngine;

namespace DiceGame.Managers
{
    public class GameEventPropagator : MonoBehaviour
    {
        [SerializeField] private AllGameEventListenerCollection allGameEventListenerCollection;

        private DiceController _currentTurnDice;
        
        private bool _interrupt;
        
        public bool Interrupt
        {
            get => _interrupt;
            set => _interrupt = value;
        }

        //TODO: Need to figure out a way to replace this RelicController reference with an Interface instead.
        //Can't use the IAllGameEventListener one, because when you get component to find the delegate interface implementation on the same object, it finds itself
        //Causing a stack overflow.
        private List<IAllGameEventListener> ListOfPartyRelics => allGameEventListenerCollection.CollectionHashset.ToList();

        #region GameEvents
        
        public void OnRelicDieResult(List<int> val)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                foreach (var value in val)
                {
                    relic.OnRelicDieResultRolled(value);
                }
            }
        }
        
        #region EncounterEvents

        [ContextMenu("Fake Encounter Start")]
        public void OnEncounterStart()
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;
                
                relic.OnEncounterStart();
            }
        }

        public void OnEncounterEnd()
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;
                
                relic.OnEncounterEnd();
            }        
        }
        #endregion

        #region CombatEvents

        public void OnPartyTurnStart(PartyManager partyManager)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;

                relic.OnPartyTurnStart(partyManager);
            }
        }

        public void OnPartyTurnEnd(PartyManager partyManager)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;

                relic.OnPartyTurnEnd(partyManager);
            }
        }

        public void OnEnemyTurnStart(MonsterManager monsterManager)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;

                 relic.OnEnemyTurnStart(monsterManager);
            }
        }

        public void OnEnemyTurnEnd(MonsterManager monsterManager)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;

                relic.OnEnemyTurnEnd(monsterManager);
            }
        }

        public void OnDealDamage(IDamageable target, IDamageDealer owner)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;

                relic.OnDealDamage(target, owner);
            }
        }

        public void OnBlock(IDamageable target, IDamageDealer owner)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;

                relic.OnBlock(target, owner);
            }
        }
        #endregion

        #region DiceEvents
        
        public void OnDiceRolled(IDiceOwner diceOwner, List<DiceController> diceControllers)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;

                relic.OnDiceRolled(diceOwner, diceControllers);
            }
        }

        public void OnDiceSelected(DiceController diceController)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;

                relic.OnDiceSelected(diceController);
            }
        }

        public void OnDiceAttachToSlot(DiceController diceController, DiceSlotHolder diceSlotHolder)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;

                relic.OnDiceAttachToSlot(diceController, diceSlotHolder);
            }
        }

        public void OnConfirmAllDie(List<DiceController> diceControllers)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;

                relic.OnConfirmAllDie(diceControllers);
            }
        }
        #endregion

        #region PartyEvents
        
        public void OnPartyCreated(PartyManager partyManager)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;

                relic.OnPartyCreated(partyManager);
            }
        }

        public void OnPartyDeath(PartyManager partyManager)
        {
            foreach (var relic in ListOfPartyRelics)
            {
                if (_interrupt) break;

                relic.OnPartyDeath(partyManager);
            }
        }
        #endregion

        
        #endregion

    }
}