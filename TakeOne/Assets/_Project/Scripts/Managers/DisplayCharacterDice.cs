using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceGame.Managers;
using DiceGame.ScriptableObjects.Dice;
using DiceGame.ScriptableObjects;

namespace DiceGame.Dice
{
    public class DisplayCharacterDice : MonoBehaviour
    {
        private GameObject[] characterDice;
        private PartyManager _partyManager;
        public GameObject dicePrefab;
        private HeroSO ownerHero;
        public Transform diceSpawnHolder;

        public GameObject[] CharacterDice { get => characterDice; set => characterDice = value; }


        private void Awake()
        {
            _partyManager = FindObjectOfType<PartyManager>();
            characterDice = new GameObject[8];
        }

        public void PopulateDice(HeroSO hero)
        {
            int i = 0;
            ownerHero = hero;
            foreach (DiceSO die in hero.CharacterDice)
            {
                DiceShaderHandler singleDie = Instantiate(dicePrefab, diceSpawnHolder.GetChild(i++)).GetComponent<DiceShaderHandler>();
                singleDie.UpdateDiceFaceTextures(die.DieSides);
            }

        }
    }
}
