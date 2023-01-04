using System.Collections;
using System.Collections.Generic;
using DiceGame.ScriptableObjects;
using UnityEngine;
using DiceGame.Dice;

namespace DiceGame
{
    public class PartyManager : MonoBehaviour
    {
        private Object[] allHeroes;
        //TODO: Fix publics, fix naming conventions, seperate get/set into multiple lines, grouping fields
        //move functions to appripriate scripts, automation
        private List<HeroSO> partyMembers = new List<HeroSO>();
        [SerializeField] private int lifePool;
        private MonsterManager monsterManager;
        private UIManager uIManager;
        private DiceManager diceMan;
        private TurnManager turnManager;

        private int currentTurn;

        public List<HeroSO> PartyMembers 
        { 
            get => partyMembers; 
            set => partyMembers = value; 
        }
        public int LifePool 
        { 
            get => lifePool; 
            set => lifePool = value; 
        }
        public TurnManager TurnManager 
        { 
            get => turnManager; 
            set => turnManager = value; 
        }
        public UIManager UIManager 
        {
            get => uIManager; 
            set => uIManager = value; 
        }
        public DiceManager DiceMan 
        { 
            get => diceMan; 
            set => diceMan = value; 
        }
        private void Awake()
        {
            uIManager = GameObject.FindObjectOfType<UIManager>();
            monsterManager = GameObject.FindObjectOfType<MonsterManager>();
            diceMan = GameObject.FindObjectOfType<DiceManager>();
            turnManager = GameObject.FindObjectOfType<TurnManager>();
            allHeroes = Resources.LoadAll("Heros", typeof(HeroSO));
        }

        // Start is called before the first frame update
        void Start()
        {
            CreateParty();
            DiceMan.CharacterSoStats = partyMembers[currentTurn];
            currentTurn = 0;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void FinishHeroActions()
        {
            if (turnManager.IsPlayerTurn)
            {
                if (currentTurn < partyMembers.Count-1)
                {
                    UIManager.EnableUIElement(UIManager.RollDice);
                    currentTurn++;
                    DiceMan.CharacterSoStats = partyMembers[currentTurn];
                }
                else
                {
                    //disableRolling();
                    UIManager.EnableUIElement(UIManager.ConfirmAll);
                }
            }
        }
        //public void disableRolling()
        //{
        //    _UIManager.disableUIElement(_UIManager.rollDice);

        //}
        public void ConfirmAllDice()
        {
            currentTurn = 0;
            //TODO: Use Dice
            DiceMan.StopAllCoroutines();
            foreach (GameObject heldDice in DiceMan.SelectedDice) 
            {
                Destroy(heldDice);
            }
            DiceMan.SelectedDice.Clear();
            DiceMan.CharacterSoStats = partyMembers[currentTurn];

            turnManager.EndTurn();
            StartCoroutine(monsterManager.PlayAnimations(1));

        }
        [ContextMenu("create Party")]
        public void CreateParty()
        {
            if (allHeroes != null)
            {
                partyMembers.Clear();
            }
            int PartySize = Random.Range(2, 5);
            for (int i = 0; i < PartySize; i++)
            {
                int pickHero = Random.Range(0, allHeroes.Length);
                partyMembers.Add((HeroSO)allHeroes[pickHero]);
            }
            foreach (HeroSO Hero in partyMembers)
            {
                lifePool += Hero.LifeMod;
            }


        }
    }
}
