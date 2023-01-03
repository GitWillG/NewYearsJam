using System.Collections;
using System.Collections.Generic;
using DiceGame.ScriptableObjects;
using UnityEngine;
using DiceGame.Dice;

namespace DiceGame
{
    public class PartyManager : MonoBehaviour
    {

        //TODO: Fix publics, fix naming conventions, seperate get/set into multiple lines, grouping fields
        //move functions to appripriate scripts, automation
        [SerializeField] private List<HeroSO> partyMembers;
        [SerializeField] private int lifePool;
        public UIManager _UIManager;
        public DiceManager _diceMan;
        private int currentTurn;
        [SerializeField] private TurnManager turnManager;

        public List<HeroSO> PartyMembers { get => partyMembers; set => partyMembers = value; }
        public int LifePool { get => lifePool; set => lifePool = value; }
        public TurnManager TurnManager { get => turnManager; set => turnManager = value; }

        // Start is called before the first frame update
        void Start()
        {
            _diceMan.CharacterSoStats = partyMembers[currentTurn];
            foreach (HeroSO Hero in partyMembers)
            {
                lifePool += Hero.LifeMod;
            }
            currentTurn = 0;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void finishHeroActions()
        {
            if (turnManager.IsPlayerTurn)
            {
                if (currentTurn < partyMembers.Count-1)
                {
                    _UIManager.enableUIElement(_UIManager.RollDice);
                    currentTurn++;
                    _diceMan.CharacterSoStats = partyMembers[currentTurn];
                }
                else
                {
                    //disableRolling();
                    _UIManager.enableUIElement(_UIManager.ConfirmAll);
                }
            }
        }
        //public void disableRolling()
        //{
        //    _UIManager.disableUIElement(_UIManager.rollDice);

        //}
        public void confirmAllDice()
        {
            currentTurn = 0;
            //TODO: Use Dice
            _diceMan.StopAllCoroutines();
            foreach (GameObject heldDice in _diceMan.SelectedDice) 
            {
                Destroy(heldDice);
            }
            _diceMan.SelectedDice.Clear();
            _diceMan.CharacterSoStats = partyMembers[currentTurn];
            turnManager.EndTurn();

        }
    }
}
