using System.Collections.Generic;
using System.Linq;
using DiceGame.Dice;
using DiceGame.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Managers
{
    public class PartyManager : MonoBehaviour
    {
        [SerializeField] private DiceSlotHolder diceSlotHolder;

        private HeroSO[] _allHeroes;
        //TODO: Fix publics, fix naming conventions, seperate get/set into multiple lines, grouping fields
        //move functions to appripriate scripts, automation
        private List<HeroSO> _partyMembers = new List<HeroSO>();
        private int _health;
        
        private MonsterManager _monsterManager;
        private UIManager _uIManager;
        private DiceManager _diceMan;
        private TurnManager _turnManager;

        public UnityEvent onRollingFinished;
        private int _currentTurn;

        public HeroSO CurrentPartyMember => PartyMembers[CurrentTurn];

        public List<HeroSO> PartyMembers => _partyMembers;

        public int Health 
        { 
            get => _health; 
            set => _health = value; 
        }
        
        public int CurrentTurn
        {
            get => _currentTurn;
            set => _currentTurn = value;
        }

        private void Awake()
        {
            _uIManager = FindObjectOfType<UIManager>();
            _monsterManager = FindObjectOfType<MonsterManager>();
            _diceMan = FindObjectOfType<DiceManager>();
            _turnManager = FindObjectOfType<TurnManager>();
            _allHeroes = Resources.LoadAll("Heros", typeof(HeroSO)).Cast<HeroSO>().ToArray();
        }

        private void Start()
        {
            CreateParty();
            _diceMan.CharacterSoStats = _partyMembers[_currentTurn];
            _currentTurn = 0;
        }
        
        public void EndPartyTurn()
        {
            _currentTurn = 0;
            _diceMan.CharacterSoStats = CurrentPartyMember;
            _monsterManager.MonsterTakeDamage();
            _turnManager.EndTurn();
            _monsterManager.ProgressTurn();
        }

        public void TryDealDamage(int amount)
        {
            var damageNegation = CalculateDamageNegation();
            
            var totalDamage = amount - damageNegation;
            
            if(totalDamage < 1) return; // Don't take the damage


            if (_health - totalDamage <= 0)
            {
                _health = 0;
                Invoke(nameof(KillSelf), 0.2f);
                //UpdateHealthBar();
            }
            else
            {
                _health -= totalDamage;
                //UpdateHealthBar();
            }
        }

        private int CalculateDamageNegation()
        {
            if (diceSlotHolder == null) return 0;
            
            var dieRolls = diceSlotHolder.GetDiceResults();

            return dieRolls is { Count: > 0 } ? dieRolls.Sum(x => x) : 0;
        }

        public void FinishHeroActions()
        {
            if (!_turnManager.IsPlayerTurn) return;
            
            if (_currentTurn < _partyMembers.Count-1)
            {
                _uIManager.EnableUIElement(_uIManager.RollDice);
                _diceMan.CharacterSoStats = _partyMembers[_currentTurn++];
            }
            else
            {
                onRollingFinished?.Invoke();
                _uIManager.EnableUIElement(_uIManager.ConfirmAll);
            }
        }

        [ContextMenu("create Party")]
        public void CreateParty()
        {
            if (_allHeroes != null)
            {
                _partyMembers.Clear();
                _health = 0;
            }

            _partyMembers = new List<HeroSO>();
            
            int partySize = Random.Range(2, 5);
            
            for (int i = 0; i < partySize; i++)
            {
                int pickHero = Random.Range(0, _allHeroes.Length);
                _partyMembers.Add((HeroSO)_allHeroes[pickHero]);
            }
            
            foreach (HeroSO hero in _partyMembers)
            {
                _health += hero.HealthContribution;
            }
        }
        
        public void KillSelf()
        {
            CreateParty();
        }
    }
}
