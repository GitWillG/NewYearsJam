using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceGame.Managers;
using DiceGame.ScriptableObjects;

namespace DiceGame.Dice
{
    public class PartyDiceManager : MonoBehaviour
    {
        private GameObject[] characterDice;
        private PartyManager _partyManager;
        public GameObject HolderPrefab;
        public Transform partyMemberHolder;


        public GameObject[] CharacterDice { get => characterDice; set => characterDice = value; }


        private void Awake()
        {
            _partyManager = FindObjectOfType<PartyManager>();
            spawnHolders();
        }

        public void spawnHolders()
        {
            int heroNo = 0;
            foreach (HeroSO hero in _partyManager.PartyMembers)
            {
                DisplayCharacterDice diceHolder = Instantiate(HolderPrefab, partyMemberHolder.GetChild(heroNo++)).GetComponent<DisplayCharacterDice>();
                Debug.Log("spawned " + diceHolder);
                diceHolder.PopulateDice(hero);
            }
        }
    }
}
