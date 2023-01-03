using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class UIManager : MonoBehaviour
    {
        public GameObject confirmDice;
        public GameObject rollDice;
        public GameObject confirmAll;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void enableUIElement(GameObject element)
        {
            element.SetActive(true);
        }
        public void disableUIElement(GameObject element)
        {
            element.SetActive(false);
        }
       
    }
}
