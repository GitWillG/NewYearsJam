using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceGame.Dice;
using DiceGame.ScriptableObjects;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Managers
{
    public class PartyManager : MonoBehaviour
    {
        [SerializeField] private DiceSlotHolder diceSlotHolder;
        [SerializeField] private MMF_Player damageFeedbackPlayer;
        [SerializeField] private MMF_Player damageNegationFeedbackPlayer;
        [SerializeField] private Transform damageNumberTransform;
        [SerializeField] private Transform damageNegationNumberTransform;

        private HeroSO[] _allHeroes;
        //TODO: Fix publics, fix naming conventions, seperate get/set into multiple lines, grouping fields
        //move functions to appripriate scripts, automation
        private List<HeroSO> _partyMembers = new List<HeroSO>();
        private int _health;
        private int _maxHealth;
        private MMHealthBar _healthBar; 
        private TextExposer _textExposer; 

        private MonsterManager _monsterManager;
        private UIManager _uIManager;
        private DiceManager _diceMan;
        private TurnManager _turnManager;
        private int _damageNegation;

        public UnityEvent onRollingFinished;
        public UnityEvent<int> onTakeDamage;
        public UnityEvent<int> onDamageNegated;
        private int _currentTurn;

        public HeroSO CurrentPartyMember => PartyMembers[CurrentTurn];
        public HeroSO RandomPartyMember => PartyMembers[Random.Range(0, _partyMembers.Count)];

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
            _healthBar = GetComponent<MMHealthBar>();
            _allHeroes = Resources.LoadAll("Heros", typeof(HeroSO)).Cast<HeroSO>().ToArray();
        }

        private void Start()
        {
            CreateParty();
            _diceMan.CharacterSoStats = _partyMembers[_currentTurn];
            _currentTurn = 0;
            _textExposer = _healthBar.ProgressBar.GetComponent<TextExposer>();
            UpdateHealthBar();
        }
        
        public void EndPartyTurn()
        {
            StartCoroutine(EndTurnEnumerator());
        }

        private IEnumerator EndTurnEnumerator()
        {
            _damageNegation = 0;
            _currentTurn = 0;
            _diceMan.CharacterSoStats = CurrentPartyMember;
            
            yield return StartCoroutine( _monsterManager.MonsterTakeDamage());

            yield return new WaitForSeconds(.2f);
            _turnManager.EndTurn();
            _monsterManager.ProgressTurn();
            _damageNegation = CalculateDamageNegation();
        }

        public void TryDealDamage(int amount)
        {
            // var damageNegation = CalculateDamageNegation();
            
            var totalDamage = amount - _damageNegation;
            
            if (_damageNegation > 0)
            {
                damageNegationFeedbackPlayer?.PlayFeedbacks(damageNumberTransform.position, _damageNegation);
            }
            
            if(totalDamage < 1) return; // Don't take the damage
            
            damageFeedbackPlayer?.PlayFeedbacks(damageNumberTransform.position, totalDamage);

            onTakeDamage?.Invoke(totalDamage);
            onDamageNegated?.Invoke(_damageNegation);
            
            if (_health - totalDamage <= 0)
            {
                _health = 0;
                Invoke(nameof(KillSelf), 0.2f);
                UpdateHealthBar();
            }
            else
            {
                _health -= totalDamage;
                UpdateHealthBar();
            }
        }
        
        private void UpdateHealthBar()
        {
            _healthBar.UpdateBar(_health, 0, _maxHealth, true);
            _textExposer.UpdateText(_health + " / " + _maxHealth);
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
                _damageNegation = 0;
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

            _maxHealth = _health;
        }
        
        public void KillSelf()
        {
            CreateParty();
        }
    }
}
