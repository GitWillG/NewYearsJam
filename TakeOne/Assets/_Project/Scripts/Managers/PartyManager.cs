using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceGame.Dice;
using DiceGame.ScriptableObjects;
using DiceGame.Utility;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace DiceGame.Managers
{
    public class PartyManager : MonoBehaviour, IDiceOwner
    {
        [SerializeField] private TextMeshProUGUI rollsLeftText;
        [SerializeField] private DamageHandler damageHandler;
        
        private HeroSO[] _allHeroes;
        private List<HeroSO> _partyMembers = new List<HeroSO>();
        private int _health;
        private int _maxHealth;
        private MonsterManager _monsterManager;
        private UIManager _uIManager;
        private DiceManager _diceMan;
        private TurnManager _turnManager;
        private int _currentTurn;

        public UnityEvent onRollingFinished;
        
        public IDamageable Damageable => damageHandler;
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
            _allHeroes = Resources.LoadAll("Heros", typeof(HeroSO)).Cast<HeroSO>().ToArray();
            
            _diceMan.onDiceRolled.AddListener(UpdateRollsLeft);
        }

        private void UpdateRollsLeft(List<DiceController> arg0)
        {
            rollsLeftText.text = "Rolls Left : " + Mathf.Abs((_currentTurn + 1) - _partyMembers.Count) + " / " + _partyMembers.Count;
        }

        private void Start()
        {
            CreateParty();
            _diceMan.CharacterSoStats = _partyMembers[_currentTurn];
            _currentTurn = 0;
            rollsLeftText.text = "Rolls Left : " + _partyMembers.Count + " / " + _partyMembers.Count;
            Damageable.Init(_health, _maxHealth);
        }
        
        public void EndPartyTurn()
        {
            StartCoroutine(EndTurnEnumerator());
        }

        private IEnumerator EndTurnEnumerator()
        {
            _currentTurn = 0;
            _diceMan.CharacterSoStats = CurrentPartyMember;
            
            //If this kills all monsters it calls End turn which flips the turn back to player
            yield return StartCoroutine( _monsterManager.MonsterTakeDamage());

            yield return new WaitForSeconds(.2f);
            _turnManager.EndTurn();
            //ProgressTurn automatically flips the turn back to player
            //Coupled with above it double flips, resulting it it never being player turn at the end of an encounter
            _monsterManager.ProgressTurn();
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
                _partyMembers.Add(_allHeroes[pickHero]);
            }
            
            foreach (HeroSO hero in _partyMembers)
            {
                _health += hero.HealthContribution;
            }

            _maxHealth = _health;
        }
        
        public void KillSelf()
        {
            Time.timeScale = 0;
            _uIManager.RestartGame.SetActive(true);
            _uIManager.RollDice.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_diceMan != null)
            {
                _diceMan.onDiceRolled.RemoveListener(UpdateRollsLeft);
            }
        }
    }
}
