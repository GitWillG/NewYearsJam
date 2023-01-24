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


        public GameObject[] CharacterDice { get => characterDice; set => characterDice = value; }


        private void Awake()
        {
            _partyManager = FindObjectOfType<PartyManager>();
            spawnHolders();
            //this.gameObject.SetActive(false);

        }
        // Start is called before the first frame update
        void Start()
        {
            characterDice = new GameObject[8];
            //for (int i = 0; i < characterDice.Length; i++)
            //{
            //    characterDice[i] = this.transform.GetChild(i + 2).gameObject;
            //}


        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void PopulateDice(List<GameObject> dice) 
        {
            int i = 0;
            foreach(GameObject die in dice)
            {
                i++;
                DiceController singleDie = Instantiate(die, characterDice[i].transform.position, Quaternion.identity).GetComponent<DiceController>();
            }

        }

        public void spawnHolders()
        {
            int heroNo = 0;
            foreach (HeroSO hero in _partyManager.PartyMembers)
            {
                DisplayCharacterDice diceHolder = Instantiate(HolderPrefab, transform.GetChild(heroNo)).GetComponent<DisplayCharacterDice>();
                Debug.Log("spawned " + diceHolder);
                diceHolder.PopulateDice(hero.CharacterDice);
                heroNo++;
            }
        }
    }
}
