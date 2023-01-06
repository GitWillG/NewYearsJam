using System.Collections;
using System.Collections.Generic;
using DiceGame.ScriptableObjects;
using UnityEngine;
using DiceGame.Dice;
using UnityEngine.Events;

namespace DiceGame
{
    public class PartyManager : MonoBehaviour
    {
        private Object[] _allHeroes;
        //TODO: Fix publics, fix naming conventions, seperate get/set into multiple lines, grouping fields
        //move functions to appripriate scripts, automation
        private List<HeroSO> _partyMembers = new List<HeroSO>();
        [SerializeField] private int lifePool;
        
        private MonsterManager _monsterManager;
        private UIManager _uIManager;
        private DiceManager _diceMan;
        private TurnManager _turnManager;

        public UnityEvent onRollingFinished;
        private int _currentTurn;

        public HeroSO CurrentPartyMember => PartyMembers[CurrentTurn];

        public List<HeroSO> PartyMembers => _partyMembers;

        public int LifePool 
        { 
            get => lifePool; 
            set => lifePool = value; 
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
            _allHeroes = Resources.LoadAll("Heros", typeof(HeroSO));
        }

        // Start is called before the first frame update
        void Start()
        {
            CreateParty();
            _diceMan.CharacterSoStats = _partyMembers[_currentTurn];
            _currentTurn = 0;
        }
        
        public void EndPartyTurn()
        {
            _currentTurn = 0;
            _diceMan.CharacterSoStats = CurrentPartyMember;
            _turnManager.EndTurn();
            StartCoroutine(_monsterManager.PlayAnimations(1));
            _monsterManager.MonsterTakeDamage();
        }
        
        public void FinishHeroActions()
        {
            if (_turnManager.IsPlayerTurn)
            {
                if (_currentTurn < _partyMembers.Count-1)
                {
                    _uIManager.EnableUIElement(_uIManager.RollDice);
                    _diceMan.CharacterSoStats = _partyMembers[_currentTurn++];
                }
                else
                {
                    //disableRolling();
                    onRollingFinished?.Invoke();
                    _uIManager.EnableUIElement(_uIManager.ConfirmAll);
                    // _diceMan.ShouldRaycast = false;
                }
            }
        }

        [ContextMenu("create Party")]
        public void CreateParty()
        {
            if (_allHeroes != null)
            {
                _partyMembers.Clear();
                lifePool = 0;
            }
            int partySize = Random.Range(2, 5);
            for (int i = 0; i < partySize; i++)
            {
                int pickHero = Random.Range(0, _allHeroes.Length);
                _partyMembers.Add((HeroSO)_allHeroes[pickHero]);
            }
            foreach (HeroSO hero in _partyMembers)
            {
                lifePool += hero.LifeMod;
            }
        }
        public void TPK()
        {
            CreateParty();
        }
    }
}
