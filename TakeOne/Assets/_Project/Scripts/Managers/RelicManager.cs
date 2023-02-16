using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiceGame.Dice;
using DiceGame.Enemy;
using DiceGame.Relics;
using DiceGame.ScriptableObjects.Dice;
using DiceGame.Utility;
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

        private bool _interrupt;

        private List<RelicController> ListOfPartyRelics => relicControllerCollection.CollectionHashset.ToList();

        public bool Interrupt
        {
            get => _interrupt;
            set => _interrupt = value;
        }

        //Highlight all the relics that are active for the combat.

        private void Awake()
        {
            _diceRoller = FindObjectOfType<DiceRoller>();
            _partyManager = FindObjectOfType<PartyManager>();
            _monsterManager = FindObjectOfType<MonsterManager>();
            _diceSelector = FindObjectOfType<DiceSelector>();
            _diceDragger = FindObjectOfType<DiceDragger>();
        }

        [ContextMenu("Roll Relic Die")]
        public void RollCombatStartDie()
        {
            diceSlot.GetDiceResults();
            _currentTurnDice = _diceRoller.RollDie(_partyManager, diceSo);
            OnDiceRolled(_partyManager,new() { _currentTurnDice });

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

                relic.OnPartyDeath(_partyManager);
            }
        }
    }
}