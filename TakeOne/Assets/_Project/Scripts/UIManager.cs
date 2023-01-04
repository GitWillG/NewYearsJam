using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject confirmDice;
        [SerializeField] private GameObject rollDice;
        [SerializeField] private GameObject confirmAll;

        public GameObject ConfirmDice { 
            get => confirmDice; 
            set => confirmDice = value; 
        }
        public GameObject RollDice 
        { 
            get => rollDice; 
            set => rollDice = value; 
        }
        public GameObject ConfirmAll { 
            get => confirmAll; 
            set => confirmAll = value; 
        }

        public void EnableUIElement(GameObject element)
        {
            element.SetActive(true);
        }
        public void DisableUIElement(GameObject element)
        {
            element.SetActive(false);
        }
       
    }
}
