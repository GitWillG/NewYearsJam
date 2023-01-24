using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceGame.Managers;
using DiceGame.ScriptableObjects.Dice;

namespace DiceGame.Dice
{
    public class DisplayCharacterDice : MonoBehaviour
    {
        private GameObject[] characterDice;
        private PartyManager _partyManager;
        public GameObject dicePrefab;

        public GameObject[] CharacterDice { get => characterDice; set => characterDice = value; }


        private void Awake()
        {
            _partyManager = FindObjectOfType<PartyManager>();
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

        public void PopulateDice(List<DiceSO> dice)
        {
            int i = 1;
            foreach (DiceSO die in dice)
            {
                i++;
                DiceController singleDie = Instantiate(dicePrefab, transform.GetChild(i)).GetComponent<DiceController>();
                singleDie.Initialize(die);
                singleDie.disableRB();
                //DiceController singleDie = Instantiate(die, characterDice[i].transform.position, Quaternion.identity).GetComponent<DiceController>();
            }

        }
    }
}
